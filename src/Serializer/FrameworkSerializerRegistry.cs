using System;
using System.Collections.Concurrent;

namespace MongoDB.FrameworkSerializer
{
    public static class FrameworkSerializerRegistry
    {
        public static string Key { get; } = "__typeAlias";

        private static readonly ConcurrentDictionary<string, Type> TypeMaps =
            new ConcurrentDictionary<string, Type>();

        public static void Map(string alias, Type type)
        {
            TypeMaps.AddOrUpdate(alias, type, (s, t) => t);
        }

        public static Type Get(string alias)
        {
            if (TypeMaps.TryGetValue(alias, out var type))
            {
                return type;
            }

            throw new InvalidOperationException($"Type for alias \"{alias}\" not registred");
        }
    }
}