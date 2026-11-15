using ImGuiNET;
using OpenTK.Windowing.Common;

/// <summary>
/// Class used to display a debug window when using Millia2D.
/// </summary>
public class DebugWindow
{
    public void OnUpdateFrame(FrameEventArgs e)
    {
        ImGui.Begin("Millia2D Debugger");
        Performance(e);
        ImGui.End();
    }

    private void Performance(FrameEventArgs e)
    {
        float fps = 1 / (float)e.Time;
        ImGui.Text($"FPS: {Math.Round(fps)}");
    }
}
