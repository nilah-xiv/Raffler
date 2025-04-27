using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using Raffler.Windows;
using Raffler.Data;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;


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
    [PluginService] internal static IChatGui ChatGui { get; private set; } = null!;

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

        ChatGui.ChatMessage += OnChatMessage;


        WindowSystem.AddWindow(ConfigWindow);
        WindowSystem.AddWindow(MainWindow);
        WindowSystem.AddWindow(ticketListWindow);

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "Open the raffle window"
        });

        PluginInterface.UiBuilder.Draw += DrawUI;
        PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUI;
        PluginInterface.UiBuilder.OpenMainUi += ToggleMainUI;
        PluginInterface.UiBuilder.OpenMainUi += TicketListUI;

        LoadEntries();

        Log.Information($"=== Loaded {PluginInterface.Manifest.Name} ===");
    }

    public void Dispose()
    {
        WindowSystem.RemoveAllWindows();

        ConfigWindow.Dispose();
        MainWindow.Dispose();
        ticketListWindow.Dispose();

        ChatGui.ChatMessage -= OnChatMessage;

        CommandManager.RemoveHandler(CommandName);
    }

    private void OnCommand(string command, string args)
    {
        ToggleMainUI();
    }

    private void DrawUI() => WindowSystem.Draw();
    public void ToggleConfigUI() => ConfigWindow.Toggle();
    public void ToggleMainUI() => MainWindow.Toggle();
    public void TicketListUI() => ticketListWindow.Toggle();

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

    private void OnChatMessage(XivChatType type, int timestamp, ref SeString sender, ref SeString message, ref bool isHandled)
    {
        var text = message.TextValue;

        if (type == XivChatType.TellIncoming) // or TellOutgoing if you want
        {
            if (text.Contains("raffle", StringComparison.OrdinalIgnoreCase) ||
                text.Contains("ticket", StringComparison.OrdinalIgnoreCase) ||
                text.Contains("join", StringComparison.OrdinalIgnoreCase))
            {
                Log.Information($"[Raffler] Detected keyword from {sender.TextValue}: {text}");
            }
        }
    }



}
