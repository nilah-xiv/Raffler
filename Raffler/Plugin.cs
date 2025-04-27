using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Raffler.Windows;
using Raffler.Data;
using System.Collections.Generic;
using System.Text.Json;
using System;
using System.IO;
using System.Linq;
using Dalamud.Game.Text.SeStringHandling.Payloads;

namespace Raffler;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static ITextureProvider TextureProvider { get; private set; } = null!;
    [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
    [PluginService] internal static IClientState ClientState { get; private set; } = null!;
    [PluginService] internal static IObjectTable ObjectTable { get; private set; } = null!;
    [PluginService] internal static IDataManager DataManager { get; private set; } = null!;
    [PluginService] internal static IPluginLog Log { get; private set; } = null!;
    [PluginService] internal static ITargetManager TargetManager { get; private set; } = null!;
    [PluginService] internal static IChatGui ChatGui { get; private set; } = null!;

    private const string CommandName = "/raffler";

    public Configuration Configuration { get; init; }
    public readonly WindowSystem WindowSystem = new("Raffler");

    private ConfigWindow ConfigWindow { get; init; }
    private MainWindow MainWindow { get; init; }
    private TicketListWindow TicketListWindow { get; init; }

    public List<TicketEntry> Entries { get; private set; } = new();
    public int BonusTicketsRemaining { get; set; } = 0;

    private string TicketSavePath => Path.Combine(PluginInterface.ConfigDirectory.FullName, "raffle_entries.json");

    private readonly string[] keywordList = { "raffle", "ticket", "join" };

    public Plugin()
    {
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

        var iconImagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory!.FullName, "raffler.png");
        ConfigWindow = new ConfigWindow(this);
        TicketListWindow = new TicketListWindow(this);
        MainWindow = new MainWindow(this, iconImagePath, TicketListWindow);

        BonusTicketsRemaining = Configuration.BogoBonusTickets;

        ChatGui.ChatMessage += OnChatMessage;

        WindowSystem.AddWindow(ConfigWindow);
        WindowSystem.AddWindow(MainWindow);
        WindowSystem.AddWindow(TicketListWindow);

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "Open Raffler UI."
        });

        PluginInterface.UiBuilder.Draw += DrawUI;
        PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUI;
        PluginInterface.UiBuilder.OpenMainUi += ToggleMainUI;
        PluginInterface.UiBuilder.OpenMainUi += TicketListUI;

        LoadEntries();

        Log.Information($"=== Raffler plugin {PluginInterface.Manifest} loaded! ===");
    }

    public void Dispose()
    {
        WindowSystem.RemoveAllWindows();

        ConfigWindow.Dispose();
        MainWindow.Dispose();
        TicketListWindow.Dispose();

        ChatGui.ChatMessage -= OnChatMessage;

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

    private void OnCommand(string command, string args) => ToggleMainUI();

    private void DrawUI() => WindowSystem.Draw();

    public void ToggleConfigUI() => ConfigWindow.Toggle();
    public void ToggleMainUI() => MainWindow.Toggle();
    public void TicketListUI() => TicketListWindow.Toggle();

    private void OnChatMessage(XivChatType type, int timestamp, ref SeString sender, ref SeString message, ref bool isHandled)
    {
        if (type is XivChatType.TellIncoming or XivChatType.TellOutgoing)
        {
            var msgText = message.TextValue;
            if (keywordList.Any(keyword => msgText.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
            {
                var payloadList = new List<Payload>
            {
                new TextPayload(sender.TextValue),
                new TextPayload(" wants a 🎟️!")
            };

                var newMsg = new SeString(payloadList);
                ChatGui.Print(newMsg);
            }
        }
    }

}
