using ImGuiNET;
using Millia2d.Profiling;
using Millia2D;
using OpenTK.Windowing.Common;

/// <summary>
/// Class used to display a debug window when using Millia2D.
/// </summary>
internal class DebugWindow
{
    // TODO: Figure out how to programmatically set docking
    internal void OnUpdateFrame(FrameEventArgs e)
    {
        GameData();

        // Keep performance last so we always see FPS data when we start debug menu
        Performance(e);
    }

    private void Performance(FrameEventArgs e)
    {
        ImGui.Begin("Performance");

        float fps = 1 / (float)e.Time;
        ImGui.Text($"FPS: {Math.Round(fps)}");

        ImGui.End();
    }

    private void GameData()
    {
        ImGui.Begin("Debugger");

        MilliaGame game = MilliaGame.GameInstance;
        ImGui.Text($"Loaded worlds: {game.LoadedScenes.Count}");
        ImGui.Text($"Total actors: {game.LoadedScenes.Sum(world => world.Actors.Count)}");

        ImGui.End();
    }
}
