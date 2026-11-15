namespace Millia2d;

using Millia2d.Logging;
using Serilog;
using Serilog.Core;

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
