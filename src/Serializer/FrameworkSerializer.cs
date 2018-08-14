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
            context.Reader.ReadStartDocument();

            SerializationInfo info = new SerializationInfo(
                args.NominalType, new FormatterConverter());

            while (context.Reader.ReadBsonType() != BsonType.EndOfDocument)
            {
                string name = context.Reader.ReadName();
                BsonType type = context.Reader.GetCurrentBsonType();
                object value;

                if (type == BsonType.Document)
                {
                    Type currentType = context.Reader
                        .FindDocumentType(args.NominalType);

                    var serializer = BsonSerializer
                        .LookupSerializer(currentType);

                    var currentContext = BsonDeserializationContext
                        .CreateRoot(context.Reader);

                    value = serializer.Deserialize(
                        currentContext,
                        new BsonDeserializationArgs
                        {
                            NominalType = currentType
                        });
                }
                else
                {
                    value = ValueSerializer
                        .Deserialize(context.Reader);
                }

                info.AddValue(name, value);
            }

            context.Reader.ReadEndDocument();

            return args.NominalType
                .InvokeSerializableConstructor(info);
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
                        .LookupSerializer(entry.ObjectType);

                    serializer.Serialize(context, args, entry.Value);
                }
                else
                {
                    ValueSerializer
                        .Serialize(context.Writer, entry.Value);
                }
            }

            context.Writer.WriteEndDocument();
        }

        public Type ValueType => typeof(T);
    }
}