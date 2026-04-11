using Microsoft.Extensions.Logging;
using System;

namespace MintCart.Logging
{
	public static class MintCartLogHelper
    {
        #region Public methods
        public static void Debug(this ILogger logger, string message, params object[] args)
        {
            logger.LogDebug(message, args);
        }
        public static void Debug(this Serilog.ILogger logger, string message, params object[] args)
        {
            logger.Debug(message, args);
        }
        public static void Error(this ILogger logger, string message, params object[] args)
        {
            logger.LogError(message, args);
        }
        public static void Error(this Serilog.ILogger logger, string message, params object[] args)
        {
            logger.Error(message, args);
        }
        public static void Error(this ILogger logger, Exception exception, string message, params object[] args)
        {
            logger.LogError(exception, message, args);
        }
        public static void Error(this Serilog.ILogger logger, Exception exception, string message, params object[] args)
        {
            logger.Error(exception, message, args);
        }
        public static void Info(this ILogger logger, string message, params object[] args)
        {
            logger.LogInformation(message, args);
        }
        public static void Info(this Serilog.ILogger logger, string message, params object[] args)
        {
            logger.Information(message, args);
        }
        public static void Warning(this ILogger logger, string message, params object[] args)
        {
            logger.LogWarning(message, args);
        }
        public static void StartMethodLog(this ILogger logger, string message, params object[] args)
        {
            logger.LogInformation($"----------------------------------Start executing method : {message}---------------------------------------", args);
        }
        public static void EndMethodLog(this ILogger logger, string message, params object[] args)
        {
            logger.LogInformation($"----------------------------------End executing method : {message}---------------------------------------", args);
        }
        public static void StartMethodtLog<T>(string message, params object[] args)
        {
            var logger = GetLoggerFromContext<T>();
            logger.Info($"----------------------------------Start executing method : {message}---------------------------------------", args);
        }
        public static void EndMethodtLog<T>(string message, params object[] args)
        {
            var logger = GetLoggerFromContext<T>();
            logger.Info($"----------------------------------End executing method : {message}---------------------------------------", args);
        }
        public static void Info<T>(string message, params object[] args)
        {
            var logger = GetLoggerFromContext<T>();
            logger.Info(message, args);
        }
        public static void Info(string message, params object[] args)
        {
            Serilog.Log.Logger.Info(message, args);
        }
        public static void Error<T>(Exception exception, string message, params object[] args)
        {
            var logger = GetLoggerFromContext<T>();
            logger.Error(exception, message, args);
        }
        public static void Error(Exception exception, string message, params object[] args)
        {
            Serilog.Log.Logger.Error(exception, message, args);
        }
        public static void Debug<T>(string message, params object[] args)
        {
            var logger = GetLoggerFromContext<T>();
            logger.Debug(message, args);
        }
        public static void Debug(string message, params object[] args)
        {
            Serilog.Log.Logger.Debug(message, args);
        }
        #endregion

        #region Private Methods
        private static Serilog.ILogger GetLoggerFromContext<T>()
        {
            return Serilog.Log.ForContext<T>();
        }
        #endregion
    }
}
 