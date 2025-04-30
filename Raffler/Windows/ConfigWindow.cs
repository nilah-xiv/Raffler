using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace Raffler.Windows;

public class ConfigWindow : Window, IDisposable
{
    private Configuration Configuration;

    public ConfigWindow(Plugin plugin) : base("Raffler Config###With a constant ID")
    {
        Flags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
                ImGuiWindowFlags.NoScrollWithMouse;

        Size = new Vector2(232, 110);
        SizeCondition = ImGuiCond.Always;

        Configuration = plugin.Configuration;

        // Ensure default values are set if config is new
        if (Configuration.Version == 0)
        {
            Configuration.ShowDebugOption = true;
            Configuration.StartingPotMillions = 10;
        }
    }

    public void Dispose() { }

    public override void PreDraw()
    {
        if (Configuration.IsConfigWindowMovable)
        {
            Flags &= ~ImGuiWindowFlags.NoMove;
        }
        else
        {
            Flags |= ImGuiWindowFlags.NoMove;
        }
    }

    public override void Draw()
    {
        Raffler.UI.RafflerTheme.Push();

        var showDebugToggle = Configuration.ShowDebugOption;
        if (ImGui.Checkbox("Enable Debug Mode", ref showDebugToggle))
        {
            Configuration.ShowDebugOption = showDebugToggle;
            Configuration.Save();
        }

        var movable = Configuration.IsConfigWindowMovable;
        if (ImGui.Checkbox("Movable Config Window", ref movable))
        {
            Configuration.IsConfigWindowMovable = movable;
            Configuration.Save();
        }

        int startingPot = Configuration.StartingPotMillions;
        if (ImGui.InputInt("Starting Pot (mil)", ref startingPot))
        {
            Configuration.StartingPotMillions = startingPot;
            Configuration.Save();
        }

        Raffler.UI.RafflerTheme.Pop();
    }
}

// Add this to Configuration.cs:
/*
public bool ShowDebugOption { get; set; } = true;
public int StartingPotMillions { get; set; } = 10;
*/
