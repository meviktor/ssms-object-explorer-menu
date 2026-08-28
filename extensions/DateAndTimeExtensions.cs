using System;

namespace SSMSObjectExplorerMenu.extensions
{
    internal static class DateTimeExtensions
    {
        extension (DateTime)
        {
            internal static DateTime TodayUtc
            {
                get
                {
                    var utcNow = DateTime.UtcNow;
                    return new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, 0, 0, 0);
                }
            }
        }
    }

    internal static class DateTimeOffsetExtensions
    {
        extension (DateTimeOffset)
        {
            internal static DateTimeOffset TodayUtc
            {
                get
                {
                    var utcNow = DateTimeOffset.UtcNow;
                    return new DateTimeOffset(utcNow.Year, utcNow.Month, utcNow.Day, 0, 0, 0, TimeSpan.Zero);
                }
            }
        }
    }
}
