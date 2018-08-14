using System;
using System.Collections.Concurrent;
using System.Linq;
using MongoDB.Bson.Serialization;

namespace MongoDB.FrameworkSerializer
{
    public static class FrameworkSerializerRegistry
    {
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
            
            var aliasType = TypeNameDiscriminator.GetActualType(alias);
            if (aliasType == null)
            {
                aliasType = BsonClassMap
                    .GetRegisteredClassMaps()
                    .SelectMany(x => x.AllMemberMaps)
                    .Where(x => x.MemberType.FullName?.EndsWith(alias) ?? false)
                    .Select(x => x.MemberType)
                    .FirstOrDefault();
            }

            if (aliasType != null)
            {
                return TypeMaps.GetOrAdd(alias, aliasType);
            }

            throw new InvalidOperationException(
                $"Type for alias \"{alias}\" not registred");
        }
    }
}