using System;
using System.Collections.Concurrent;
using System.Runtime.Serialization;
using MongoDB.Bson.Serialization;

namespace MongoDB.FrameworkSerializer
{
    public class FrameworkSerializerProvider : IBsonSerializationProvider
    {
        public static FrameworkSerializerProvider Instance { get; }
            = new FrameworkSerializerProvider();

        private readonly ConcurrentDictionary<Type, IBsonSerializer> _serializers =
            new ConcurrentDictionary<Type, IBsonSerializer>();

        public IBsonSerializer GetSerializer(Type type)
        {
            if (_serializers.TryGetValue(type, out var value))
            {
                return value;
            }

            if (typeof(ISerializable).IsAssignableFrom(type))
            {
                var serializerType = typeof(FrameworkSerializer<>).MakeGenericType(type);
                var serializer = serializerType.GetConstructor(new Type[0]).Invoke(new object[0]);

                return _serializers.GetOrAdd(type, (IBsonSerializer)serializer);
            }

            return null;
        }
    }
}