using System;
using ViccosLite.Core.Domain.Users;

namespace ViccosLite.Core.Domain.Logging
{
    public class Log : BaseEntity
    {
        public int LogLevelId { get; set; }

        public LogLevel LogLevel
        {
            get { return (LogLevel) LogLevelId; }
            set { LogLevelId = (int) value; }
        }

        public int? UserId { get; set; }
        public virtual User User { get; set; }
        public string ReferrerUrl { get; set; }
        public string PageUrl { get; set; }
        public string IpAddress { get; set; }
        public string ShortMessage { get; set; }
        public string FullMessage { get; set; }
        public DateTime CreatedOnUtc { get; set; }
    }
}