using MongoDB.Bson;

namespace MongoDB.FrameworkSerializer
{
    internal static class BsonValueExtensions
    {
        internal static object ToNative(this BsonValue bsonValue)
        {
            if (bsonValue.IsString)
            {
                return bsonValue.AsString;
            }

            if (bsonValue.IsBoolean)
            {
                return bsonValue.AsBoolean;
            }

            if (bsonValue.IsGuid)
            {
                return bsonValue.AsGuid;
            }

            if (bsonValue.IsInt32)
            {
                return bsonValue.AsInt32;
            }

            if (bsonValue.IsInt64)
            {
                return bsonValue.AsInt64;
            }

            if (bsonValue.IsDouble)
            {
                return bsonValue.AsDouble;
            }

            if (bsonValue.IsGuid)
            {
                return bsonValue.AsGuid;
            }

            if (bsonValue.IsObjectId)
            {
                return bsonValue.AsObjectId;
            }

            if (bsonValue.IsValidDateTime)
            {
                return bsonValue.ToUniversalTime();
            }

            return bsonValue;
        }
    }
}