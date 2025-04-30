using ImGuiNET;

namespace Raffler.UI;

public static class RafflerTheme
{
    private static int _styleColorCount = 0;
    private static int _styleVarCount = 0;

    public static void Push()
    {
        ImGui.StyleColorsDark();
        var style = ImGui.GetStyle();
        var colors = style.Colors;

        var neonTeal = new System.Numerics.Vector4(0.0f, 1.0f, 1.0f, 1.0f);
        var darkBackground = new System.Numerics.Vector4(0.06f, 0.06f, 0.08f, 1.0f);
        var darkHeader = new System.Numerics.Vector4(0.10f, 0.10f, 0.12f, 1.0f);

        ImGui.PushStyleColor(ImGuiCol.WindowBg, darkBackground); _styleColorCount++;
        ImGui.PushStyleColor(ImGuiCol.ChildBg, darkBackground); _styleColorCount++;
        ImGui.PushStyleColor(ImGuiCol.PopupBg, darkBackground); _styleColorCount++;

        ImGui.PushStyleColor(ImGuiCol.Header, neonTeal * new System.Numerics.Vector4(1, 1, 1, 0.4f)); _styleColorCount++;
        ImGui.PushStyleColor(ImGuiCol.HeaderHovered, neonTeal * new System.Numerics.Vector4(1, 1, 1, 0.7f)); _styleColorCount++;
        ImGui.PushStyleColor(ImGuiCol.HeaderActive, neonTeal); _styleColorCount++;

        ImGui.PushStyleColor(ImGuiCol.Button, neonTeal * new System.Numerics.Vector4(1, 1, 1, 0.4f)); _styleColorCount++;
        ImGui.PushStyleColor(ImGuiCol.ButtonHovered, neonTeal * new System.Numerics.Vector4(1, 1, 1, 0.7f)); _styleColorCount++;
        ImGui.PushStyleColor(ImGuiCol.ButtonActive, neonTeal); _styleColorCount++;

        ImGui.PushStyleColor(ImGuiCol.FrameBg, darkHeader); _styleColorCount++;
        ImGui.PushStyleColor(ImGuiCol.FrameBgHovered, neonTeal * new System.Numerics.Vector4(1, 1, 1, 0.25f)); _styleColorCount++;
        ImGui.PushStyleColor(ImGuiCol.FrameBgActive, neonTeal * new System.Numerics.Vector4(1, 1, 1, 0.4f)); _styleColorCount++;

        ImGui.PushStyleColor(ImGuiCol.Tab, darkHeader); _styleColorCount++;
        ImGui.PushStyleColor(ImGuiCol.TabHovered, neonTeal * new System.Numerics.Vector4(1, 1, 1, 0.6f)); _styleColorCount++;
        ImGui.PushStyleColor(ImGuiCol.TabActive, neonTeal * new System.Numerics.Vector4(1, 1, 1, 0.8f)); _styleColorCount++;

        ImGui.PushStyleColor(ImGuiCol.TitleBg, darkHeader); _styleColorCount++;
        ImGui.PushStyleColor(ImGuiCol.TitleBgActive, neonTeal * new System.Numerics.Vector4(1, 1, 1, 0.6f)); _styleColorCount++;

        ImGui.PushStyleColor(ImGuiCol.SliderGrab, neonTeal * new System.Numerics.Vector4(1, 1, 1, 0.7f)); _styleColorCount++;
        ImGui.PushStyleColor(ImGuiCol.SliderGrabActive, neonTeal); _styleColorCount++;
        ImGui.PushStyleColor(ImGuiCol.CheckMark, neonTeal); _styleColorCount++;

        ImGui.PushStyleColor(ImGuiCol.Separator, neonTeal * new System.Numerics.Vector4(1, 1, 1, 0.3f)); _styleColorCount++;
        ImGui.PushStyleColor(ImGuiCol.SeparatorHovered, neonTeal * new System.Numerics.Vector4(1, 1, 1, 0.7f)); _styleColorCount++;
        ImGui.PushStyleColor(ImGuiCol.SeparatorActive, neonTeal); _styleColorCount++;

        ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 6.0f); _styleVarCount++;
        ImGui.PushStyleVar(ImGuiStyleVar.ChildRounding, 6.0f); _styleVarCount++;
        ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 5.0f); _styleVarCount++;
        ImGui.PushStyleVar(ImGuiStyleVar.GrabRounding, 5.0f); _styleVarCount++;
        ImGui.PushStyleVar(ImGuiStyleVar.TabRounding, 5.0f); _styleVarCount++;
        ImGui.PushStyleVar(ImGuiStyleVar.FrameBorderSize, 1f); _styleVarCount++;
        ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 1f); _styleVarCount++;
        ImGui.PushStyleVar(ImGuiStyleVar.PopupBorderSize, 1f); _styleVarCount++;
    }

    public static void Pop()
    {
        ImGui.PopStyleColor(_styleColorCount);
        ImGui.PopStyleVar(_styleVarCount);
        _styleColorCount = 0;
        _styleVarCount = 0;
    }
}
