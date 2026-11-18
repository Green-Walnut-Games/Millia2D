using System.Runtime.InteropServices;
using ImGuiNET;
using Millia2D;
using Millia2d;
using Millia2d.Profiling;
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

        LiveMeasurements();
    }

    private void Performance(FrameEventArgs e)
    {
        ImGui.Begin("Performance");

        float fps = 1 / (float)e.Time;
        ImGui.Text($"FPS: {Math.Round(fps)}");

        ImGui.Text("");

        ImGui.End();
    }

    private void LiveMeasurements()
    {
        ImGui.Begin("Live measurements");

        ImGui.Text("Live measurements:");
        foreach (var pair in Profiler.LiveMeasurements)
        {
            Log.Info($"ImGui text should be updated with {pair.Value.LastValue}");
            ImGui.Text($"{pair.Key}: {pair.Value.LastValue}");
            PlotGraphForTimeSeries(pair.Key, pair.Value);
        }

        ImGui.End();
    }

    private void PlotGraphForTimeSeries(string message, TimeSeriesData data)
    {
        ImGui.PlotLines(
            label: message,
            values: ref data.values[0],
            values_count: data.values.Length,
            values_offset: data.offset,
            overlay_text: $"",
            scale_min: data.values.Min(),
            scale_max: data.values.Max(),
            graph_size: new System.Numerics.Vector2(0, 80)
        );
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

public struct TimeSeriesData
{
    public float[] values { get; internal set; }
    public int offset { get; internal set; }
    public float LastValue
    {
        get => values[values.Length - 1];
    }

    public TimeSeriesData()
    {
        values = new float[200];
        offset = 0;
    }
}
