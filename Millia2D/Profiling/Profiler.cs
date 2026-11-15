using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Millia2d.Profiling;

internal static class Profiler
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

        float fivePercentLowFrameTime = fivePercentLowFrameTimes.Average();
        float fivePercentHighFrameTime = fivePercentHighFrameTimes.Average();

        return (fivePercentLowFrameTime, fivePercentHighFrameTime);
    }
}
