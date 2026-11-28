using System.Diagnostics;
using System.Text;

namespace Millia2D.Profiling;

/// <summary>
/// Class useful for performing profiling of your games.
/// </summary>
public static class Profiler
{
    internal static List<float> frameTimes = new();

    internal static void AddFrameTimeValue(float value)
    {
        frameTimes.Add(value);
    }

    internal static string Report()
    {
        frameTimes.Order();

        float minFrameTime = frameTimes.Min();
        float maxFrameTime = frameTimes.Max();
        float averageFrameTime = frameTimes.Average();

        var fivePercent = CalculatePercentLow(5);
        var onePercent = CalculatePercentLow(1);
        var noughtOnePercent = CalculatePercentLow(0.1f);

        StringBuilder str = new();
        str.AppendLine("=== Millia2D Performance Report ===");

        str.AppendLine($"Min frame time: {minFrameTime} (FPS: {1 / minFrameTime})");
        str.AppendLine($"Max frame time: {maxFrameTime} (FPS: {1 / maxFrameTime})");
        str.AppendLine($"Average frame time: {averageFrameTime} (FPS: {1 / averageFrameTime})");

        str.AppendLine($"=== Percent Lows ===");
        str.AppendLine(
            $"5% low: {fivePercent.Item1} (FPS: {1 / fivePercent.Item1}) 5% high: {fivePercent.Item2} (FPS: {1 / fivePercent.Item2})"
        );
        str.AppendLine(
            $"1% low: {onePercent.Item1} (FPS: {1 / onePercent.Item1}) 1% high: {onePercent.Item2} (FPS: {1 / onePercent.Item2})"
        );
        str.AppendLine(
            $"0.1% low: {noughtOnePercent.Item1} (FPS: {1 / noughtOnePercent.Item1}) 0.1% high: {noughtOnePercent.Item2} (FPS: {1 / noughtOnePercent.Item2})"
        );

        return str.ToString();
    }

    private static (float, float) CalculatePercentLow(float percent = 5)
    {
        // The average frame time for the lowest 5% frame times
        int fivePercentLowNumber = (int)MathF.Round(frameTimes.Count / 100 * percent);

        List<float> fivePercentLowFrameTimes = new();
        for (int i = 0; i < fivePercentLowNumber; i++)
        {
            fivePercentLowFrameTimes.Add(frameTimes[i]);
        }

        List<float> fivePercentHighFrameTimes = new();
        for (int i = frameTimes.Count - fivePercentLowNumber; i < frameTimes.Count; i++)
        {
            fivePercentHighFrameTimes.Add(frameTimes[i]);
        }

        float fivePercentLowFrameTime =
            fivePercentLowFrameTimes.Count > 0 ? fivePercentLowFrameTimes.Average() : 0;
        float fivePercentHighFrameTime =
            fivePercentHighFrameTimes.Count > 0 ? fivePercentHighFrameTimes.Average() : 0;

        return (fivePercentLowFrameTime, fivePercentHighFrameTime);
    }

    /// <summary>
    /// Performs the given action, logs the time taken to do so and returns
    /// the time taken.
    /// </summary>
    /// <param name="measurementName"></param>
    /// <param name="action"></param>
    /// <returns>Time taken to perform said action, in ms.</returns>
    public static double MeasureTime(Action action)
    {
        Stopwatch watch = new();
        watch.Start();
        action.Invoke();
        watch.Stop();

        return watch.ElapsedMilliseconds;
    }

    internal static Dictionary<string, TimeSeriesData> LiveMeasurements { get; private set; } =
        new();

    internal static Dictionary<string, TimeSeriesData> EngineLiveMeasurements
    {
        get;
        private set;
    } = new();

    internal static void AddLiveMeasurement(
        string message,
        double value,
        Dictionary<string, TimeSeriesData> target
    )
    {
        if (target.ContainsKey(message))
        {
            TimeSeriesData data = target[message];
            data.values[data.offset] = (float)value;
            data.offset = data.offset + 1;

            if (data.offset >= data.values.Length)
            {
                data.offset = 0;
            }

            target[message] = data;
        }
        else
        {
            TimeSeriesData data = new();
            data.values[0] = (float)value;
            data.offset = 0;

            target.Add(message, data);
        }
    }

    internal static Dictionary<string, object> LiveData = new();
    internal static Dictionary<string, object> EngineLiveData = new();

    internal static void AddLiveData(
        string message,
        object value,
        Dictionary<string, object> target
    )
    {
        if (target.ContainsKey(message))
        {
            target[message] = value;
        }
        else
        {
            target.Add(message, value);
        }
    }
}
