namespace Millia2D.Logging;

/// <summary>
/// Minimum level at which the logging should work.
/// eg. If level is set to Warning, Debug or Info messages will not appear, and only Warnings and Errors will.
/// </summary>
public enum LogLevel
{
    Debug,
    Info,
    Warning,
    Error,
}
