using Microsoft.Extensions.Logging;

namespace MusicKG.Scheduler.Engine.Extensions
{
    public static class LoggerExtensions
    {
        public static void LogActionInfo(this ILogger logger, string actionName, string message)
        {
            logger?.LogInformation($"[{actionName}]: {message}");
        }

        public static void LogActionError(this ILogger logger, string actionName, string message)
        {
            logger?.LogError($"[{actionName}]: {message}");
        }

        public static void LogActionWarning(this ILogger logger, string actionName, string message)
        {
            logger?.LogWarning($"[{actionName}]: {message}");
        }
    }
}
