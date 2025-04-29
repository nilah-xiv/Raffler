using ImGuiNET;

namespace Raffler.UI;

public static class RafflerTheme
{
    public static void Apply()
    {
        var style = ImGui.GetStyle();
        var colors = style.Colors;

        // Set the basic dark theme first
        ImGui.StyleColorsDark();

        // Core Raffler Colors (Neon Teal and Dark Charcoal)
        var neonTeal = new System.Numerics.Vector4(0.0f, 1.0f, 1.0f, 1.0f); // Pure neon teal
        var darkBackground = new System.Numerics.Vector4(0.06f, 0.06f, 0.08f, 1.0f); // Slightly richer black
        var darkHeader = new System.Numerics.Vector4(0.10f, 0.10f, 0.12f, 1.0f);

        // Backgrounds
        colors[(int)ImGuiCol.WindowBg] = darkBackground;
        colors[(int)ImGuiCol.ChildBg] = darkBackground;
        colors[(int)ImGuiCol.PopupBg] = darkBackground;

        // Headers
        colors[(int)ImGuiCol.Header] = neonTeal * new System.Numerics.Vector4(1f, 1f, 1f, 0.4f);
        colors[(int)ImGuiCol.HeaderHovered] = neonTeal * new System.Numerics.Vector4(1f, 1f, 1f, 0.7f);
        colors[(int)ImGuiCol.HeaderActive] = neonTeal;

        // Buttons
        colors[(int)ImGuiCol.Button] = neonTeal * new System.Numerics.Vector4(1f, 1f, 1f, 0.4f);
        colors[(int)ImGuiCol.ButtonHovered] = neonTeal * new System.Numerics.Vector4(1f, 1f, 1f, 0.7f);
        colors[(int)ImGuiCol.ButtonActive] = neonTeal;

        // Frame Backgrounds (e.g., text input)
        colors[(int)ImGuiCol.FrameBg] = darkHeader;
        colors[(int)ImGuiCol.FrameBgHovered] = neonTeal * new System.Numerics.Vector4(1f, 1f, 1f, 0.25f);
        colors[(int)ImGuiCol.FrameBgActive] = neonTeal * new System.Numerics.Vector4(1f, 1f, 1f, 0.4f);

        // Tabs
        colors[(int)ImGuiCol.Tab] = darkHeader;
        colors[(int)ImGuiCol.TabHovered] = neonTeal * new System.Numerics.Vector4(1f, 1f, 1f, 0.6f);
        colors[(int)ImGuiCol.TabActive] = neonTeal * new System.Numerics.Vector4(1f, 1f, 1f, 0.8f);

        // Titles
        colors[(int)ImGuiCol.TitleBg] = darkHeader;
        colors[(int)ImGuiCol.TitleBgActive] = neonTeal * new System.Numerics.Vector4(1f, 1f, 1f, 0.6f);

        // Sliders and Checks
        colors[(int)ImGuiCol.SliderGrab] = neonTeal * new System.Numerics.Vector4(1f, 1f, 1f, 0.7f);
        colors[(int)ImGuiCol.SliderGrabActive] = neonTeal;
        colors[(int)ImGuiCol.CheckMark] = neonTeal;

        // Separators
        colors[(int)ImGuiCol.Separator] = neonTeal * new System.Numerics.Vector4(1f, 1f, 1f, 0.3f);
        colors[(int)ImGuiCol.SeparatorHovered] = neonTeal * new System.Numerics.Vector4(1f, 1f, 1f, 0.7f);
        colors[(int)ImGuiCol.SeparatorActive] = neonTeal;

        // Minor style tweaks for extra slickness
        style.WindowRounding = 6.0f;
        style.ChildRounding = 6.0f;
        style.FrameRounding = 5.0f;
        style.GrabRounding = 5.0f;
        style.TabRounding = 5.0f;

        style.FrameBorderSize = 1f;
        style.WindowBorderSize = 1f;
        style.PopupBorderSize = 1f;
    }
}
