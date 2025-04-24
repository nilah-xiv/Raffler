using Dalamud.Interface.Windowing;
using ImGuiNET;
using System;
using System.Numerics;

namespace Raffler.Windows;

public class TicketListWindow : Window, IDisposable
{
    public TicketListWindow() : base("Raffle Tickets ###raffleTickets")
    {
        Flags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse;
        Size = new Vector2(300, 400);
        SizeCondition = ImGuiCond.FirstUseEver;
    }

    public void Dispose() { }

    public override void Draw()
    {
        ImGui.Text("This is the raffle ticket window.");
    }
}
