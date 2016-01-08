using System;
using System.Threading;
using ViccosLite.Core.Domain.Logging;
using ViccosLite.Core.Domain.Users;

namespace ViccosLite.Services.Logging
{
    public static class LoggingExtensions
    {
        public static void Debug(this ILogger logger, string message, Exception exception = null,
            User user = null)
        {
            FilteredLog(logger, LogLevel.Debug, message, exception, user);
        }

        public static void Information(this ILogger logger, string message, Exception exception = null,
            User user = null)
        {
            FilteredLog(logger, LogLevel.Information, message, exception, user);
        }

        public static void Warning(this ILogger logger, string message, Exception exception = null,
            User user = null)
        {
            FilteredLog(logger, LogLevel.Warning, message, exception, user);
        }

        public static void Error(this ILogger logger, string message, Exception exception = null,
            User user = null)
        {
            FilteredLog(logger, LogLevel.Error, message, exception, user);
        }

        public static void Fatal(this ILogger logger, string message, Exception exception = null,
            User user = null)
        {
            FilteredLog(logger, LogLevel.Fatal, message, exception, user);
        }

        private static void FilteredLog(ILogger logger, LogLevel level, string message, Exception exception = null,
            User user = null)
        {
            //No se guarda el registro cuando es de un hilo de una excepcion de abort
            if (exception is ThreadAbortException)
                return;

            if (logger.IsEnabled(level))
            {
                var fullMessage = exception == null ? string.Empty : exception.ToString();
                logger.InsertLog(level, message, fullMessage, user);
            }
        }
    }
}