using System;
using System.Collections;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

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
                else if(type == BsonType.Array)
                {
                    value = BsonArraySerializer.Instance.Deserialize(
                        context, args).ToDotNetValue();
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
                else if (entry.Value is IEnumerable values)
                {
                    // TODO: For the moment the Mongo-Enumerable serializer
                    // TODO: is not writing the entire namespace of the collection type
                    // TODO: to the document.

                    // TODO: Find a better way.
                    BsonArraySerializer.Instance.Serialize(
                        context, args, new BsonArray(values));
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