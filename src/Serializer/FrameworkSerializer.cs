using System;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.IO;
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
            TypeBuilder builder = new TypeBuilder();
            context.Reader.ReadStartDocument();

            while(context.Reader.ReadBsonType() != BsonType.EndOfDocument)
            {
                string name = context.Reader.ReadName();
                BsonType type = context.Reader.GetCurrentBsonType();
                object value;

                if (type == BsonType.Document)
                {
                    value = DeserializeCore(context, args);
                }
                else
                {
                    value = ValueSerializer
                        .Deserialize(context);
                }

                builder.AddField(name, value);
            }

            context.Reader.ReadEndDocument();

            return builder.Build();
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            Serialize(context, args, (T)value);
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, T value)
        {
            context.Writer.WriteStartDocument();
            context.Writer.WriteTypeInformation(value);

            SerializationInfo info = ((ISerializable)value)
                .GetSerializationInfo();

            foreach (SerializationEntry entry in info)
            {
                context.Writer.WriteName(entry.Name);

                if (entry.Value is ISerializable)
                {
                    var serializer = BsonSerializer
                        .SerializerRegistry
                        .GetSerializer(entry.ObjectType);

                    serializer.Serialize(context, args, entry.Value);
                }
                else
                {
                    ValueSerializer
                        .Serialize(context, entry.Value);
                }
            }

            context.Writer.WriteEndDocument();
        }

        public Type ValueType => typeof(T);
    }
}