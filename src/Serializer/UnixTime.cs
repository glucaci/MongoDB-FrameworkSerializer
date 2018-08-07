using System;

namespace MongoDB.FrameworkSerializer
{
    internal static class UnixTime
    {
        private static readonly DateTime UnixEpoch =
            new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        internal static DateTime ToDateTime(long unixTime)
        {
            return UnixEpoch.AddSeconds(unixTime);
        }

        internal static long ToMilliseconds(DateTime date)
        {
            return (date - UnixEpoch).Milliseconds;
        }
    }
}