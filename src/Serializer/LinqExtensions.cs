using System;
using System.Collections;
using System.Runtime.Serialization;

namespace MongoDB.FrameworkSerializer
{
    internal static class LinqExtensions
    {
        internal static void ForEach(
            this IEnumerable collection,
            Action<object> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }

        internal static void ForEach(
            this SerializationInfo serializationInfo,
            Action<SerializationEntry> action)
        {
            foreach (var item in serializationInfo)
            {
                action(item);
            }
        }
    }
}