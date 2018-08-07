using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace MongoDB.FrameworkSerializer
{
    internal static class ValueSerializer
    {
        internal static object Deserialize(BsonDeserializationContext context)
        {
            var bsonReader = context.Reader;

            var bsonType = bsonReader.GetCurrentBsonType();
            switch (bsonType)
            {
                case BsonType.Boolean:
                    return bsonReader.ReadBoolean();

                case BsonType.DateTime:
                    return UnixTime.ToDateTime(bsonReader.ReadDateTime());

                case BsonType.Int32:
                    return bsonReader.ReadInt32();

                case BsonType.Int64:
                    return bsonReader.ReadInt64();

                case BsonType.Double:
                    return bsonReader.ReadDouble();

                case BsonType.Null:
                    return null;

                case BsonType.ObjectId:
                    return bsonReader.ReadObjectId();

                case BsonType.String:
                    return bsonReader.ReadString();

                default:
                    throw new InvalidOperationException(
                        $"Cannot deserialize {bsonType} to native value.");
            }
        }

        internal static void Serialize(BsonSerializationContext context, object value)
        {
            switch (value)
            {
                case bool boolValue:
                    context.Writer.WriteBoolean(boolValue);
                    break;

                case DateTime dateTimeValue:
                    context.Writer.WriteDateTime(UnixTime.ToMilliseconds(dateTimeValue));
                    break;

                case int intValue:
                    context.Writer.WriteInt32(intValue);
                    break;

                case long longValue:
                    context.Writer.WriteInt64(longValue);
                    break;

                case double doubleValue:
                    context.Writer.WriteDouble(doubleValue);
                    break;

                case null:
                    context.Writer.WriteNull();
                    break;

                case ObjectId objeecIdValue:
                    context.Writer.WriteObjectId(objeecIdValue);
                    break;

                case string stringValue:
                    context.Writer.WriteString(stringValue);
                    break;

                default:
                    throw new InvalidOperationException(
                        $"Cannot serialize type ${value.GetType().Name}");
            }
        }
    }
}
