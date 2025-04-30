using Dalamud.Interface.Windowing;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Raffler.Windows;

public class TicketListWindow : Window, IDisposable
{
    private bool showDiscordView = false;
    private string discordHeader;
    private readonly Plugin plugin;

    public TicketListWindow(Plugin plugin) : base("Raffle Tickets ###raffleTickets")
    {
        this.plugin = plugin;

        Flags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse;
        Size = new Vector2(300, 400);
        SizeCondition = ImGuiCond.FirstUseEver;

        // Use configured pot for default header
        int startingPot = plugin.Configuration.StartingPotMillions;
        discordHeader = $"RAFFLE {DateTime.Now:M/d/yy} — {startingPot}MIL STARTING POT";
    }

    public void Dispose() { }

    public override void Draw()
    {
        Raffler.UI.RafflerTheme.Push();

        if (plugin.Entries.Count == 0)
        {
            ImGui.TextUnformatted("🎟 No entries yet!");
            Raffler.UI.RafflerTheme.Pop();
            return;
        }

        ImGui.Checkbox("🧾 Show Discord Style View", ref showDiscordView);

        if (showDiscordView)
        {
            ImGui.InputText("Discord Header", ref discordHeader, 128);
            ImGui.Separator();
            ImGui.TextUnformatted(discordHeader);

            int ticketNum = 1;
            foreach (var entry in plugin.Entries)
            {
                for (int i = 0; i < entry.TotalTickets; i++)
                {
                    ImGui.TextUnformatted($"{ticketNum++,3}  {entry.PlayerName}");
                }
            }

            ImGui.Separator();

            if (ImGui.Button("📋 Copy This View to Clipboard"))
            {
                var lines = new List<string> { discordHeader };
                int i = 1;
                foreach (var entry in plugin.Entries)
                {
                    for (int t = 0; t < entry.TotalTickets; t++)
                        lines.Add($"{i++,3}  {entry.PlayerName}");
                }

                ImGui.SetClipboardText(string.Join("\n", lines));
            }
        }

        ImGui.TextUnformatted($"📋 Raffle Entries ({plugin.Entries.Count})");
        ImGui.Separator();

        foreach (var entry in plugin.Entries)
        {
            ImGui.TextUnformatted(entry.ToString());
        }

        ImGui.Separator();
        if (ImGui.Button("📋 Export Full List"))
        {
            var output = new List<string>();
            int i = 1;
            foreach (var entry in plugin.Entries)
            {
                for (int t = 0; t < entry.TotalTickets; t++)
                    output.Add($"{i++} {entry.PlayerName}");
            }
            ImGui.SetClipboardText(string.Join("\n", output));
        }

        ImGui.Spacing();
        if (ImGui.Button("📋 Export Grouped List"))
        {
            var output = new List<string>();
            int ticketNum = 1;
            foreach (var entry in plugin.Entries)
            {
                int start = ticketNum;
                int end = ticketNum + entry.TotalTickets - 1;
                output.Add(entry.TotalTickets == 1
                    ? $"{start} {entry.PlayerName}"
                    : $"{start}-{end} {entry.PlayerName}");
                ticketNum = end + 1;
            }
            ImGui.SetClipboardText(string.Join("\n", output));
        }

        ImGui.Spacing();
        if (ImGui.Button("📋 Copy to Clipboard"))
        {
            var export = string.Join("\n", plugin.Entries.Select(e => e.ToString()));
            ImGui.SetClipboardText(export);
        }

        Raffler.UI.RafflerTheme.Pop();
    }
}
