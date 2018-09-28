using System;
using System.Runtime.Serialization;
using MongoDB.Bson.Serialization;

namespace MongoDB.FrameworkSerializer
{
    internal class FrameworkSerializer<T> : IBsonSerializer<T>
    {
        object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return DeserializeCore(context, args);
        }

        public T Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return (T)DeserializeCore(context, args);
        }

        private ISerializable DeserializeCore(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return Deserializer.Deserialize(context, args);
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            Serialize(context, args, (T)value);
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, T value)
        {
            Serializer.Serialize(context, args, value);
        }

        public Type ValueType => typeof(T);
    }
}