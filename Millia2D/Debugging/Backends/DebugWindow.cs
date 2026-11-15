using ImGuiNET;
using Millia2D;
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
        GameData();
        ImGui.End();
    }

    private void Performance(FrameEventArgs e)
    {
        float fps = 1 / (float)e.Time;
        ImGui.Text($"FPS: {Math.Round(fps)}");
    }

    private void GameData()
    {
        MilliaGame game = MilliaGame.GameInstance;
        ImGui.Text($"Loaded worlds: {game.LoadedScenes.Count}");
        ImGui.Text($"Total actors: {game.LoadedScenes.Sum(world => world.Actors.Count)}");
    }
}
