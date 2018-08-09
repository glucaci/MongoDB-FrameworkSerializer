using System;
using System.Collections.Concurrent;
using System.Runtime.Serialization;
using MongoDB.Bson.Serialization;

namespace MongoDB.FrameworkSerializer
{
    public class FrameworkSerializerProvider : IBsonSerializationProvider
    {
        private static readonly object[] FrameworkSerializerParameters
            = new object[0];

        private static readonly Type[] FrameworkSerializerConstructorTypes
            = new Type[0];

        public static FrameworkSerializerProvider Instance { get; }
            = new FrameworkSerializerProvider();

        private readonly ConcurrentDictionary<Type, IBsonSerializer> _serializers =
            new ConcurrentDictionary<Type, IBsonSerializer>();

        public IBsonSerializer GetSerializer(Type type)
        {
            FrameworkSerializerRegistry.Map(type.FullName, type);

            if (_serializers.TryGetValue(type, out var value))
            {
                return value;
            }

            if (typeof(ISerializable).IsAssignableFrom(type))
            {
                var serializerType = typeof(FrameworkSerializer<>)
                    .MakeGenericType(type);
                
                var serializer = serializerType
                    .GetConstructor(FrameworkSerializerConstructorTypes)
                    .Invoke(FrameworkSerializerParameters);

                return _serializers
                    .GetOrAdd(type, (IBsonSerializer)serializer);
            }

            return null;
        }
    }
}