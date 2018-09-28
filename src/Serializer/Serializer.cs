using System.Collections;
using System.Runtime.Serialization;
using MongoDB.Bson.Serialization;

namespace MongoDB.FrameworkSerializer
{
    internal static class Serializer
    {
        internal static void Serialize<T>(
            BsonSerializationContext context,
            BsonSerializationArgs args,
            T value)
        {
            context.Writer.WriteStartDocument();
            context.Writer.WriteTypeInformation(value);

            ((ISerializable)value)
                .GetSerializationInfo()
                .ForEach(entry =>
                {
                    context.Writer.WriteName(entry.Name);
                    SerializeObject(context, args, entry.Value);
                });

            context.Writer.WriteEndDocument();
        }

        private static void SerializeObject(
            BsonSerializationContext context,
            BsonSerializationArgs args,
            object value)
        {
            switch (value)
            {
                case ISerializable serializableValue:
                    SerializeISerializable(context, args, serializableValue);
                    break;

                case ICollection items:
                    SerializeICollection(context, args, items);
                    break;

                default:
                    SerializeValue(context, value);
                    break;
            }
        }

        private static void SerializeISerializable(
            BsonSerializationContext context,
            BsonSerializationArgs args,
            object value)
        {
            var serializer = BsonSerializer
                .LookupSerializer(value.GetType());

            serializer.Serialize(context, args, value);
        }

        private static void SerializeICollection(
            BsonSerializationContext context,
            BsonSerializationArgs args,
            ICollection collection)
        {
            context.Writer.WriteStartArray();

            collection
                .ForEach(item => SerializeObject(context, args, item));

            context.Writer.WriteEndArray();
        }

        private static void SerializeValue(
            BsonSerializationContext context,
            object value)
        {
            ValueSerializer
                .Serialize(context.Writer, value);
        }
    }
}