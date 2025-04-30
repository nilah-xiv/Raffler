using System;
using System.Linq;
using System.Numerics;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using Dalamud.Game.ClientState.Objects.Types;
using ImGuiNET;
using Lumina.Excel.Sheets;
using Raffler.Data;

namespace Raffler.Windows;

public class MainWindow : Window, IDisposable
{
    private string RafflerImg;
    private Plugin Plugin;
    private readonly TicketListWindow ticketListWindow;
    private int ticketCount = 1;
    private string playerName = "";

    public MainWindow(Plugin plugin, string raffleimgarg, TicketListWindow ticketListWindow)
        : base("Raffler##WithHiddenID", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        Size = new Vector2(460, 560);
        SizeCondition = ImGuiCond.FirstUseEver;
        Position = new Vector2(600, 200);
        PositionCondition = ImGuiCond.FirstUseEver;

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
        Raffler.UI.RafflerTheme.Push();

        using (var child = ImRaii.Child("Content", Vector2.Zero, true))
        {
            if (!child.Success)
                return;

            ImGui.TextUnformatted("Welcome to Raffler!");
            var raffleImgTexture = Plugin.TextureProvider.GetFromFile(RafflerImg).GetWrapOrDefault();
            if (raffleImgTexture != null)
            {
                using (ImRaii.PushIndent(55f))
                {
                    ImGui.Image(raffleImgTexture.ImGuiHandle, new Vector2(192, 192));
                }
            }

            ImGuiHelpers.ScaledDummy(20.0f);

            var newTerritoryId = Plugin.ClientState.TerritoryType;
            if (Plugin.DataManager.GetExcelSheet<TerritoryType>().TryGetRow(newTerritoryId, out var territoryRow))
            {
                var house = territoryRow.PlaceName.Value.Name.ExtractText();
                var houseShort = house.Split(' ').FirstOrDefault() ?? house;
                ImGui.TextUnformatted($"Zone: ({newTerritoryId}) {houseShort}");
            }

            ImGui.Separator();
            ImGui.TextUnformatted("\uD83C\uDFAF Raffle Settings");

            var config = Plugin.Configuration;

            if (ImGui.BeginCombo("BOGO Type", config.RaffleBogoType.ToString()))
            {
                foreach (var value in Enum.GetValues<BogoType>())
                {
                    if (ImGui.Selectable(value.ToString(), value == config.RaffleBogoType))
                    {
                        config.RaffleBogoType = value;
                        config.Save();
                    }
                }
                ImGui.EndCombo();
            }

            bool locked = Plugin.Entries.Count > 0;

            if (locked)
                ImGui.BeginDisabled();

            if (config.RaffleBogoType == BogoType.Custom)
            {
                ImGui.SetNextItemWidth(180f);
                if (ImGui.InputInt("Custom Bonus Tickets", ref config.BogoBonusTickets))
                {
                    config.BogoBonusTickets = Math.Max(0, config.BogoBonusTickets);
                    config.Save();
                }
            }

            ImGui.SetNextItemWidth(180f);
            int bogoSessionLimit = config.BogoSessionLimit;
            if (ImGui.InputInt("BOGO Session Limit", ref bogoSessionLimit))
            {
                config.BogoSessionLimit = Math.Max(0, bogoSessionLimit);
                config.Save();
            }

            ImGui.SetNextItemWidth(180f);
            float costK = config.TicketCost / 1000f;
            if (ImGui.InputFloat("Ticket Cost (k)", ref costK, 1.0f, 5.0f, "%.0f"))
            {
                config.TicketCost = costK * 1000f;
                config.Save();
            }

            if (locked)
                ImGui.EndDisabled();

            if (locked)
                ImGui.TextColored(new Vector4(1f, 0.6f, 0.3f, 1f), "\uD83D\uDD12 Bonus Tickets locked after first entry");

            if (locked)
                ImGui.BeginDisabled();

            int potMil = config.StartingPotMillions;
            if (ImGui.InputInt("Starting Pot (mil)", ref potMil))
            {
                config.StartingPotMillions = Math.Max(0, potMil);
                config.Save();
            }

            if (locked)
                ImGui.EndDisabled();

            ImGui.Separator();
            ImGui.TextUnformatted("\uD83C\uDFAB Ticket Entry");

            ImGui.InputText("Player Name", ref playerName, 64);

            ImGui.SameLine();
            if (ImGui.Button("@"))
            {
                var target = Plugin.TargetManager.Target;
                if (target != null && target.ObjectKind == Dalamud.Game.ClientState.Objects.Enums.ObjectKind.Player)
                {
                    playerName = target.Name.TextValue;
                }
            }

            ImGui.InputInt("Tickets Requested", ref ticketCount);
            ticketCount = Math.Max(1, ticketCount);

            var totalCost = ticketCount * config.TicketCost;
            int bonusTickets = 0;

            switch (config.RaffleBogoType)
            {
                case BogoType.None:
                    bonusTickets = 0;
                    break;
                case BogoType.Custom:
                    bonusTickets = config.BogoBonusTickets;
                    break;
                case BogoType.Buy1Get1:
                    bonusTickets = ticketCount;
                    break;
                case BogoType.LimitedSupply:
                    bonusTickets = Math.Min(ticketCount, Plugin.BonusTicketsRemaining);
                    break;
            }

            bonusTickets = Math.Min(bonusTickets, Plugin.BonusTicketsRemaining);

            ImGui.TextUnformatted($"➡️ Cost: {totalCost:N0} gil");
            ImGui.TextUnformatted($"🎁 Bonus Tickets: {bonusTickets}");
            ImGui.TextUnformatted($"🎟️ Total Tickets: {ticketCount + bonusTickets}");

            if (ImGui.Button("✅ Confirm Entry"))
            {
                if (!string.IsNullOrWhiteSpace(playerName))
                {
                    if (Plugin.Entries.Count == 0)
                    {
                        Plugin.Configuration.StartingGil = (int)(ticketCount * config.TicketCost);
                        Plugin.SessionStartTime = DateTime.Now;
                        Plugin.SaveEntries();
                    }

                    var entry = new TicketEntry
                    {
                        PlayerName = playerName.Trim(),
                        BaseTickets = ticketCount,
                        BonusTickets = bonusTickets
                    };

                    Plugin.Entries.Add(entry);
                    Plugin.BonusTicketsRemaining = Math.Max(0, Plugin.BonusTicketsRemaining - bonusTickets);
                    Plugin.SaveEntries();
                }
                else
                {
                    ImGui.TextColored(new Vector4(1f, 0.2f, 0.2f, 1f), "⚠ Please enter a player name.");
                }
            }

            ImGui.Separator();

            if (ImGui.Button("📋 Current Ticket List"))
            {
                Plugin.TicketListUI();
            }

            ImGui.SameLine();
            if (ImGui.Button("🧹 Reset Raffle Session"))
            {
                Plugin.Entries.Clear();
                Plugin.BonusTicketsRemaining = Plugin.Configuration.BogoBonusTickets;
                Plugin.Configuration.StartingGil = 0;
                Plugin.SaveEntries();
                Plugin.SessionStartTime = DateTime.Now;
            }

            var sessionTickets = Plugin.Entries.Sum(e => e.BaseTickets + e.BonusTickets);
            var gilMade = Plugin.Entries.Sum(e => e.BaseTickets * config.TicketCost);
            var ticketsPerHour = sessionTickets / Math.Max((DateTime.Now - Plugin.SessionStartTime).TotalHours, 0.01);

            ImGui.TextColored(new Vector4(1f, 0.4f, 0.4f, 1f), $"🎟 Tickets Sold (Session): {sessionTickets} ({gilMade:N0} gil made)");

            if (ImGui.IsItemHovered())
            {
                var profit = gilMade - config.StartingGil;

                ImGui.BeginTooltip();
                ImGui.Text($"💰 Current Gil: {gilMade:N0}");
                ImGui.Text($"📦 Starting Amount: {config.StartingGil:N0}");
                ImGui.Text($"📈 Profit: {profit:N0}");
                ImGui.Text($"🕒 Tickets per hour: {ticketsPerHour:F2}");
                ImGui.EndTooltip();
            }
        }

        Raffler.UI.RafflerTheme.Pop();
    }
}
