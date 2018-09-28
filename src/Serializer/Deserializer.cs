using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

namespace MongoDB.FrameworkSerializer
{
    internal static class Deserializer
    {
        internal static ISerializable Deserialize(
            BsonDeserializationContext context,
            BsonDeserializationArgs args)
        {
            context.Reader.ReadStartDocument();

            SerializationInfo info = new SerializationInfo(
                args.NominalType, new FormatterConverter());

            while (IsEndOfDocument(context))
            {
                string name = context.Reader.ReadName();
                BsonType type = context.Reader.GetCurrentBsonType();
                object value;

                switch (type)
                {
                    case BsonType.Document:
                        value = DeserializeDocument(context, args);
                        break;

                    case BsonType.Array:
                        value = DeserializeArray(context, args);
                        break;

                    default:
                        value = DeserializeValue(context);
                        break;
                }
                
                info.AddValue(name, value);
            }

            context.Reader.ReadEndDocument();

            return args.NominalType
                .InvokeSerializableConstructor(info);
        }

        private static object DeserializeDocument(
            BsonDeserializationContext context,
            BsonDeserializationArgs args)
        {
            Type currentType = context.Reader
                .FindDocumentType(args.NominalType);

            IBsonSerializer serializer = BsonSerializer
                .LookupSerializer(currentType);

            BsonDeserializationContext currentContext = BsonDeserializationContext
                .CreateRoot(context.Reader);

            return serializer.Deserialize(
                currentContext,
                new BsonDeserializationArgs
                {
                    NominalType = currentType
                });
        }

        private static object DeserializeArray(
            BsonDeserializationContext context,
            BsonDeserializationArgs args)
        {
            context.Reader.ReadStartArray();

            List<object> items = ReadArrayItems(context, args);

            context.Reader.ReadEndArray();

            return items.ToArray();
        }

        private static object DeserializeValue(
            BsonDeserializationContext context)
        {
            return ValueSerializer
                .Deserialize(context.Reader);
        }

        private static List<object> ReadArrayItems(
            BsonDeserializationContext context,
            BsonDeserializationArgs args)
        {
            List<object> items = new List<object>();

            while (IsEndOfDocument(context))
            {
                BsonType itemType = context.Reader.GetCurrentBsonType();
                items.Add(itemType == BsonType.Document
                    ? DeserializeDocument(context, args)
                    : DeserializeValue(context));
            }

            return items;
        }

        private static bool IsEndOfDocument(
            BsonDeserializationContext context)
        {
            return context.Reader.ReadBsonType() != BsonType.EndOfDocument;
        }
    }
}