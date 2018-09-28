using System;

namespace MongoDB.FrameworkSerializer.Tests.Model
{
    internal static class String
    {
        public static bool InvariantEquals(string first, string second)
        {
            return string.Equals(
                first,
                second,
                StringComparison.InvariantCultureIgnoreCase);
        }

        public static int GetInvariantHashCode(string value)
        {
            return value != null
                ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(value) * 397
                : 0;
        }
    }
}