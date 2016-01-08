using System;
using System.Collections.ObjectModel;
using ViccosLite.Core.Domain.Users;

namespace ViccosLite.Services.Helpers
{
    public interface IDateTimeHelper
    {
        TimeZoneInfo FindTimeZoneById(string id);
        ReadOnlyCollection<TimeZoneInfo> GetSystemTimeZones();
        DateTime ConvertToUserTime(DateTime dt);
        DateTime ConvertToUserTime(DateTime dt, DateTimeKind sourceDateTimeKind);
        DateTime ConvertToUserTime(DateTime dt, TimeZoneInfo sourceTimeZone);
        DateTime ConvertToUserTime(DateTime dt, TimeZoneInfo sourceTimeZone, TimeZoneInfo destinationTimeZone);
        DateTime ConvertToUtcTime(DateTime dt);
        DateTime ConvertToUtcTime(DateTime dt, DateTimeKind sourceDateTimeKind);
        DateTime ConvertToUtcTime(DateTime dt, TimeZoneInfo sourceTimeZone);
        TimeZoneInfo GetUserTimeZone(User user);
        TimeZoneInfo DefaultStoreTimeZone { get; set; }
        TimeZoneInfo CurrentTimeZone { get; set; }

    }
}