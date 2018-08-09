using System;
using System.Globalization;
using MongoDB.Bson;
using MongoDB.Bson.IO;

namespace MongoDB.FrameworkSerializer
{
    internal static class ValueSerializer
    {
        internal static object Deserialize(IBsonReader reader)
        {
            switch (reader.GetCurrentBsonType())
            {
                case BsonType.ObjectId:
                    return reader.ReadObjectId();

                case BsonType.Boolean:
                    return reader.ReadBoolean();

                case BsonType.DateTime:
                    return UnixTime.ToDateTime(reader.ReadDateTime());

                case BsonType.Int32:
                    return reader.ReadInt32();

                case BsonType.Binary:
                    return reader.ReadBytes();

                case BsonType.Int64:
                    return reader.ReadInt64();

                case BsonType.Double:
                    return reader.ReadDouble();

                case BsonType.Null:
                    reader.ReadNull();
                    return null;

                case BsonType.String:
                    return reader.ReadString();

                default:
                    throw new InvalidOperationException(
                        $"Cannot deserialize {reader.GetCurrentBsonType()} to native value.");
            }
        }

        internal static void Serialize(IBsonWriter writer, object value)
        {
            switch (value)
            {
                case ObjectId objecId:
                    writer.WriteObjectId(objecId);
                    break;

                case bool _bool:
                    writer.WriteBoolean(_bool);
                    break;

                case byte _byte:
                    writer.WriteInt32(_byte);
                    break;

                case sbyte _sbyte:
                    writer.WriteInt32(_sbyte);
                    break;

                case char _char:
                    writer.WriteInt32(_char);
                    break;

                case decimal _decimal:
                    writer.WriteString(
                        _decimal.ToString("N" ,CultureInfo.InvariantCulture));
                    break;

                case double _double:
                    writer.WriteDouble(_double);
                    break;

                case float _float:
                    writer.WriteDouble(_float);
                    break;

                case int _int:
                    writer.WriteInt32(_int);
                    break;

                case uint _uint:
                    writer.WriteInt64(_uint);;
                    break;

                case long _long:
                    writer.WriteInt64(_long);
                    break;

                case ulong _ulong:
                    writer.WriteString(_ulong.ToString(CultureInfo.InvariantCulture));
                    break;

                case short _short:
                    writer.WriteInt32(_short);
                    break;

                case ushort _ushort:
                    writer.WriteInt32(_ushort);
                    break;

                case string _string:
                    writer.WriteString(_string);
                    break;

                case DateTime dateTime:
                    writer.WriteDateTime(
                        UnixTime.ToMilliseconds(dateTime));
                    break;

                case Guid guid:
                    writer.WriteString(guid.ToString());
                    break;

                case null:
                    writer.WriteNull();
                    break;

                default:
                    throw new InvalidOperationException(
                        $"Cannot serialize type {value.GetType().FullName}");
            }
        }
    }
}
