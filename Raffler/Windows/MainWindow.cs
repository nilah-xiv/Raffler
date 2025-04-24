using System;
using System.Numerics;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using Dalamud.Game.ClientState.Objects.Types;
using ImGuiNET;
using Lumina.Excel.Sheets;

namespace Raffler.Windows;

public class MainWindow : Window, IDisposable
{
    private string RafflerImg;
    private Plugin Plugin;
    private readonly TicketListWindow ticketListWindow;
   

    // We give this window a hidden ID using ##
    // So that the user will see "My Amazing Window" as window title,
    // but for ImGui the ID is "My Amazing Window##With a hidden ID"
    public MainWindow(Plugin plugin, string raffleimgarg, TicketListWindow ticketListWindow)
        : base("Raffler##With a hidden ID", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        RafflerImg = raffleimgarg;
        Plugin = plugin;
        this.ticketListWindow = ticketListWindow;
    }

    public void Dispose() { }

    public override void Draw()
    {
        // Do not use .Text() or any other formatted function like TextWrapped(), or SetTooltip().
        // These expect formatting parameter if any part of the text contains a "%", which we can't
        // provide through our bindings, leading to a Crash to Desktop.
        // Replacements can be found in the ImGuiHelpers Class
        ImGui.TextUnformatted($"The random config bool is {Plugin.Configuration.SomePropertyToBeSavedAndWithADefault}");

        if (ImGui.Button("Show Settings"))
        {
            Plugin.ToggleConfigUI();


        }
        if (ImGui.Button("T/L"))
        {

            Plugin.TicketListUI();

        }

        ImGui.Spacing();

        // Normally a BeginChild() would have to be followed by an unconditional EndChild(),
        // ImRaii takes care of this after the scope ends.
        // This works for all ImGui functions that require specific handling, examples are BeginTable() or Indent().

        using (var child = ImRaii.Child("SomeChildWithAScrollbar", Vector2.Zero, true))
        {
            if (child.Success)
            {
                // Draw welcome text and image first
                ImGui.TextUnformatted("Welcome to:");
                var raffleimginsert = Plugin.TextureProvider.GetFromFile(RafflerImg).GetWrapOrDefault();
                if (raffleimginsert != null)
                {
                    using (ImRaii.PushIndent(55f))
                    {
                        ImGui.Image(raffleimginsert.ImGuiHandle, new Vector2(256, 256));
                    }
                }
                else
                {
                    ImGui.TextUnformatted("Image not found.");
                }

                ImGuiHelpers.ScaledDummy(20.0f);

                // Zone / class info
                var localPlayer = Plugin.ClientState.LocalPlayer;
                if (localPlayer != null && localPlayer.ClassJob.IsValid)
                {
                    ImGui.TextUnformatted($"Our current job is ({localPlayer.ClassJob.RowId}) \"{localPlayer.ClassJob.Value.Abbreviation.ExtractText()}\"");
                }

                var territoryId = Plugin.ClientState.TerritoryType;
                if (Plugin.DataManager.GetExcelSheet<TerritoryType>().TryGetRow(territoryId, out var territoryRow))
                {
                    ImGui.TextUnformatted($"We are currently in ({territoryId}) \"{territoryRow.PlaceName.Value.Name.ExtractText()}\"");
                }

                ImGui.Separator();

                // Raffle settings *now appear below*
                ImGui.TextUnformatted("🎯 Raffle Settings");

                var config = Plugin.Configuration;

                var bogoType = config.RaffleBogoType;
                if (ImGui.BeginCombo("BOGO Type", bogoType.ToString()))
                {
                    foreach (var value in Enum.GetValues<BogoType>())
                    {
                        if (ImGui.Selectable(value.ToString(), value == bogoType))
                        {
                            config.RaffleBogoType = value;
                            config.Save();
                        }
                    }
                    ImGui.EndCombo();
                }

                int bonus = config.BogoBonusTickets;
                if (ImGui.InputInt("Bonus Tickets For the Session", ref bonus))
                {
                    config.BogoBonusTickets = Math.Max(0, bonus);
                    config.Save();
                }

                float costK = config.TicketCost / 1000f;
                if (ImGui.InputFloat("Ticket Cost (k)", ref costK, 1.0f, 5.0f, "%.0f k"))
                {
                    config.TicketCost = costK * 1000f;
                    config.Save();
                }
                ImGui.Separator();
                ImGui.TextUnformatted("🎫 Ticket Entry");

                var ticketCount = 1;
                var playerName = "";
                
                // Player name input
                ImGui.InputText("Player Name", ref playerName, 64);

                // Ticket quantity input
                ImGui.InputInt("Tickets Requested", ref ticketCount);
                ticketCount = Math.Max(1, ticketCount);

                // Live price calculation
                var basePrice = Plugin.Configuration.TicketCost;
                var totalCost = ticketCount * basePrice;

                // Bonus ticket logic
                int bonusTickets = 0;
                switch (Plugin.Configuration.RaffleBogoType)
                {
                    case BogoType.Buy1Get1:
                        bonusTickets = ticketCount;
                        break;
                    case BogoType.Buy1Get2:
                        bonusTickets = ticketCount * 2;
                        break;
                    case BogoType.EveryOther:
                        bonusTickets = ticketCount / 2;
                        break;
                    case BogoType.MaxPerPurchase:
                        bonusTickets = Plugin.Configuration.BogoBonusTickets;
                        break;
                }

                // Final summary
                ImGui.TextUnformatted($"➡️ Cost: {totalCost:N0} gil");
                ImGui.TextUnformatted($"🎁 Bonus Tickets: {bonusTickets}");
                ImGui.TextUnformatted($"🎟️ Total Tickets: {ticketCount + bonusTickets}");

                // Action button
                if (ImGui.Button("✅ Confirm Entry"))
                {
                    if (!string.IsNullOrWhiteSpace(playerName))
                    {
                        // TODO: add to ticket system
                        Plugin.Log.Info($"[Raffle] Added {playerName} - {ticketCount} + {bonusTickets} bonus = {ticketCount + bonusTickets} tickets for {totalCost:N0}g");
                        // Optionally display a little toast or sound here
                    }
                    else
                    {
                        ImGui.TextColored(new Vector4(1f, 0.2f, 0.2f, 1f), "⚠ Please enter a player name.");
                    }
                }

            }
        }
    }
}
