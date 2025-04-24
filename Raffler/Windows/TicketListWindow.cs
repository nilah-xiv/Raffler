using Dalamud.Interface.Windowing;
using ImGuiNET;
using System;
using System.Linq;
using System.Numerics;

namespace Raffler.Windows;

public class TicketListWindow : Window, IDisposable
{
    private readonly Plugin plugin;

    public TicketListWindow(Plugin plugin) : base("Raffle Tickets ###raffleTickets")
    {
        this.plugin = plugin;

        Flags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse;
        Size = new Vector2(300, 400);
        SizeCondition = ImGuiCond.FirstUseEver;
    }


    public void Dispose() { }

    public override void Draw()
    {
        if (plugin.Entries.Count == 0)
        {
            ImGui.TextUnformatted("🎟 No entries yet!");
            return;
        }

        ImGui.TextUnformatted($"📋 Raffle Entries ({plugin.Entries.Count})");
        ImGui.Separator();

        foreach (var entry in plugin.Entries)
        {
            ImGui.TextUnformatted(entry.ToString());
        }

        ImGui.Separator();

        if (ImGui.Button("📋 Copy to Clipboard"))
        {
            var export = string.Join("\n", plugin.Entries.Select(e => e.ToString()));
            ImGui.SetClipboardText(export);
        }
    }

}
