using System;
using System.Collections.Generic;
using ViccosLite.Core;
using ViccosLite.Core.Domain.Users;
using ViccosLite.Core.Domain.Logging;

namespace ViccosLite.Services.Logging
{
    public interface ILogger
    {
        bool IsEnabled(LogLevel level);
        void DeleteLog(Log log);
        void ClearLog();
        IPagedList<Log> GetAllLogs(DateTime? fromUtc, DateTime? toUtc,
            string message, LogLevel? logLevel, int pageIndex, int pageSize);
        Log GetLogById(int logId);
        IList<Log> GetLogByIds(int[] logIds);
        Log InsertLog(LogLevel logLevel, string shortMessage, string fullMessage = "", User user= null);
    }
}