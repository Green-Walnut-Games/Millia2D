using System.Runtime.InteropServices;
using ImGuiNET;
using Millia2D;
using Millia2D;
using Millia2D.Inputs;
using Millia2D.Profiling;
using OpenTK.Windowing.Common;

/// <summary>
/// Class used to display a debug window when using Millia2D.
/// </summary>
internal class DebugWindow
{
    // TODO: Figure out how to programmatically set docking
    internal void OnUpdateFrame(FrameEventArgs e)
    {
        DebuggerTab();
        InputsTab();

        // Keep performance last so we always see FPS data when we start debug menu
        PerformanceTab(e);

        LiveMeasurementsTab(Profiler.LiveMeasurements);
    }

    private void PerformanceTab(FrameEventArgs e)
    {
        ImGui.Begin("Performance");

        float fps = 1 / (float)e.Time;
        ImGui.Text($"FPS: {Math.Round(fps)}");

        ImGui.Text("");

        ShowLiveMeasurementsGraphs(Profiler.EngineLiveMeasurements);

        ImGui.End();
    }

    private void ShowLiveData(Dictionary<string, double> target)
    {
        if (target.Count > 0)
        {
            ImGui.Text("Live data: ");
            foreach (var data in target)
            {
                ImGui.Text($"{data.Key}: {data.Value}");
            }
            ImGui.Text("");
        }
    }

    private void InputsTab()
    {
        ImGui.Begin("Input");

        foreach (var val in Enum.GetValues(typeof(Keys)))
        {
            if (Input.IsKeyDown((Keys)val))
            {
                ImGui.Text($"Down: {(Keys)val}");
            }

            if (Input.IsKeyPressed((Keys)val))
            {
                ImGui.Text($"Pressed: {(Keys)val}");
            }

            if (Input.IsKeyReleased((Keys)val))
            {
                ImGui.Text($"Released: {(Keys)val}");
            }
        }

        ImGui.End();
    }

    private void LiveMeasurementsTab(Dictionary<string, TimeSeriesData> data)
    {
        ImGui.Begin("Live measurements");

        ShowLiveData(Profiler.LiveData);

        ShowLiveMeasurementsGraphs(data);

        ImGui.End();
    }

    private void ShowLiveMeasurementsGraphs(Dictionary<string, TimeSeriesData> data)
    {
        if (data.Count > 0)
        {
            ImGui.Text("Live measurements:");
            foreach (var pair in data)
            {
                PlotGraphForTimeSeries(pair.Key, pair.Value);
            }
        }
        else
        {
            ImGui.Text("You have no live measurements set up.");
        }
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

    private void DebuggerTab()
    {
        ImGui.Begin("Debugger");

        ShowLiveData(Profiler.EngineLiveData);

        MilliaGame game = MilliaGame.Instance;
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
        values = new float[1000];
        offset = 0;
    }
}
