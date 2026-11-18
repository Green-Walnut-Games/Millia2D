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
    public static Dictionary<string, TimeSeriesData> liveMeasurements { get; private set; } = new();

    public void AddLiveMeasurement(string message, double value)
    {
        if (liveMeasurements.ContainsKey(message))
        {
            TimeSeriesData data = liveMeasurements[message];
            data.values[data.offset] = (float)value;
            data.offset = (data.offset + 1);

            if (data.offset >= data.values.Length)
            {
                data.offset = 0;
            }

            liveMeasurements[message] = data;

            Log.Warning($"{message} already exists, added with offset {data.offset}");
        }
        else
        {
            Log.Warning($"{message} needs to be added");

            TimeSeriesData data = new();
            data.values[0] = (float)value;
            data.offset = 0;

            liveMeasurements.Add(message, data);
        }
    }

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

        ImGui.Text("");

        //.OrderByDescending(x =>
        //     x.Value.values[x.Value.values.Length - 1]
        // )
        ImGui.Text("Live measurements");
        foreach (var pair in liveMeasurements)
        {
            ImGui.Text($"{pair.Key}: {pair.Value.values[pair.Value.values.Length - 1]}");
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

internal struct TimeSeriesData
{
    internal float[] values;
    internal int offset;

    public TimeSeriesData()
    {
        values = new float[1000];
        offset = 0;
    }
}
