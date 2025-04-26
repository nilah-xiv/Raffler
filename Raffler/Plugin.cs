using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using Raffler.Windows;
using Dalamud.Game.ClientState.Objects;
using Raffler.Data;
using System.Collections.Generic;
using System.Text.Json;

namespace Raffler;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static ITextureProvider TextureProvider { get; private set; } = null!;
    [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
    [PluginService] internal static IClientState ClientState { get; private set; } = null!;
    [PluginService] internal static IDataManager DataManager { get; private set; } = null!;
    [PluginService] internal static IPluginLog Log { get; private set; } = null!;
    [PluginService] internal static ITargetManager TargetManager { get; private set; } = null!;

    private const string CommandName = "/raffler";

    public Configuration Configuration { get; init; }

    public readonly WindowSystem WindowSystem = new("Raffler");
    private ConfigWindow ConfigWindow { get; init; }
    private MainWindow MainWindow { get; init; }

    private TicketListWindow ticketListWindow { get; init; }
    public List<TicketEntry> Entries { get; private set; } = new();
    public int BonusTicketsRemaining { get; set; } = 0;
    private string TicketSavePath => Path.Combine(PluginInterface.ConfigDirectory.FullName, "raffle_entries.json");




    public Plugin()
    {
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

        var iconImagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "raffler.png");
        ConfigWindow = new ConfigWindow(this);
        ticketListWindow = new TicketListWindow(this);
        MainWindow = new MainWindow(this, iconImagePath, ticketListWindow);
        BonusTicketsRemaining = Configuration.BogoBonusTickets;

        WindowSystem.AddWindow(ConfigWindow);
        WindowSystem.AddWindow(MainWindow);
        WindowSystem.AddWindow(ticketListWindow);

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "A useful message to display in /xlhelp"
        });

        PluginInterface.UiBuilder.Draw += DrawUI;

        // This adds a button to the plugin installer entry of this plugin which allows
        // to toggle the display status of the configuration ui
        PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUI;

        // Adds another button that is doing the same but for the main ui of the plugin
        PluginInterface.UiBuilder.OpenMainUi += ToggleMainUI;

        PluginInterface.UiBuilder.OpenMainUi += TicketListUI;
        LoadEntries(); // Load saved tickets on plugin start

        // Add a simple message to the log with level set to information
        // Use /xllog to open the log window in-game
        // Example Output: 00:57:54.959 | INF | [SamplePlugin] ===A cool log message from Sample Plugin===
        Log.Information($"===A cooll log message from {PluginInterface.Manifest.Name}===");
    }

    public void Dispose()
    {
        WindowSystem.RemoveAllWindows();

        ConfigWindow.Dispose();
        MainWindow.Dispose();
        ticketListWindow.Dispose();
        SaveEntries(); 

        CommandManager.RemoveHandler(CommandName);
    }
    public void SaveEntries()
    {
        var json = JsonSerializer.Serialize(Entries);
        File.WriteAllText(TicketSavePath, json);
    }

    public void LoadEntries()
    {
        if (File.Exists(TicketSavePath))
        {
            var json = File.ReadAllText(TicketSavePath);
            var loaded = JsonSerializer.Deserialize<List<TicketEntry>>(json);
            if (loaded != null)
                Entries = loaded;
        }
    }

    private void OnCommand(string command, string args)
    {
        // in response to the slash command, just toggle the display status of our main ui
        ToggleMainUI();
    }

    private void DrawUI() => WindowSystem.Draw();

    public void ToggleConfigUI() => ConfigWindow.Toggle();
    public void ToggleMainUI() => MainWindow.Toggle();
    public void TicketListUI() => ticketListWindow.Toggle();
}
