namespace Millia2D;

using Millia2D.Logging;
using Millia2D.Profiling;
using Serilog;
using Serilog.Core;

/// <summary>
/// Class used to log data to the console or the debugging window.
/// </summary>
public static class Log
{
    // Verbosity levels
    // Path for engine logs
    internal static string logPath { get; } = "logs/latest.log";

    internal static LoggingLevelSwitch levelSwitch;

    internal static void Init()
    {
        // Keep latest.log as the latest log file
        if (File.Exists(logPath))
        {
            File.Delete(logPath);
        }

        levelSwitch = new();
        levelSwitch.MinimumLevel = Serilog.Events.LogEventLevel.Information;

        Serilog.Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(logPath, rollingInterval: RollingInterval.Minute)
            .WriteTo.File(logPath, rollingInterval: RollingInterval.Infinite)
            .MinimumLevel.ControlledBy(levelSwitch)
            .CreateLogger();
    }

    public static void Info(string message)
    {
        Serilog.Log.Information(message);
    }

    /// <summary>
    /// Used to display data as time graphs in the debugging window.
    /// It is advised to disable any calls to this function in a release build.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="value"></param>
    public static void LiveMeasurement(string message, double value)
    {
        Serilog.Log.Debug($"{message}: {value}");
        Profiler.AddLiveMeasurement(message, value, Profiler.LiveMeasurements);
    }

    public static void LiveData(string message, object value)
    {
        Profiler.AddLiveData(message, value, Profiler.LiveData);
    }

    internal static void LiveDataEngine(string message, object value)
    {
        Profiler.AddLiveData(message, value, Profiler.EngineLiveData);
    }

    internal static void LiveMeasurementEngine(string message, double value)
    {
        Profiler.AddLiveMeasurement(message, value, Profiler.EngineLiveMeasurements);
    }

    public static void Debug(string message)
    {
        Serilog.Log.Debug(message);
    }

    public static void Warning(string message)
    {
        Serilog.Log.Warning(message);
    }

    public static void Error(string message)
    {
        Serilog.Log.Error(message);
    }

    public static void SetLogLevel(LogLevel logLevel)
    {
        switch (logLevel)
        {
            case LogLevel.Debug:
                levelSwitch.MinimumLevel = Serilog.Events.LogEventLevel.Debug;
                break;
            case LogLevel.Info:
                levelSwitch.MinimumLevel = Serilog.Events.LogEventLevel.Information;
                break;
            case LogLevel.Warning:
                levelSwitch.MinimumLevel = Serilog.Events.LogEventLevel.Warning;
                break;
            case LogLevel.Error:
                levelSwitch.MinimumLevel = Serilog.Events.LogEventLevel.Error;
                break;
        }
    }
}
