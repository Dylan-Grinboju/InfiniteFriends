#if BEPINEX
using BepInEx.Logging;
#elif SILK
using SilkLogger = Silk.Logger;
#endif

namespace InfiniteFriends;

internal interface ILogger
{
    void LogDebug(object message);
    void LogInfo(object message);
    void LogWarning(object message);
    void LogError(object message);
    void LogFatal(object message);
}

#if BEPINEX
internal class BepInExLogger(ManualLogSource logger) : ILogger
{
    public void LogDebug(object message) => logger.LogDebug(message);
    public void LogInfo(object message) => logger.LogInfo(message);
    public void LogWarning(object message) => logger.LogWarning(message);
    public void LogError(object message) => logger.LogError(message);
    public void LogFatal(object message) => logger.LogFatal(message);
}
#elif MODWEAVER
internal class ModWeaverLogger(NLog.Logger logger) : ILogger
{
    public void LogDebug(object message) => logger.Debug(message);
    public void LogInfo(object message) => logger.Info(message);
    public void LogWarning(object message) => logger.Warn(message);
    public void LogError(object message) => logger.Error(message);
    public void LogFatal(object message) => logger.Fatal(message);
}
#elif SILK
internal class SilkLoggerWrapper : ILogger
{
    public void LogDebug(object message) => SilkLogger.Log(message?.ToString());
    public void LogInfo(object message) => SilkLogger.LogInfo(message?.ToString());
    public void LogWarning(object message) => SilkLogger.LogWarning(message?.ToString());
    public void LogError(object message) => SilkLogger.LogError(message?.ToString());
    public void LogFatal(object message) => SilkLogger.LogError($"FATAL: {message}");
}
#endif
