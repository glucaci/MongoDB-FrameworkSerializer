using System.Linq;
using MongoDB.Bson;

namespace MongoDB.FrameworkSerializer
{
    internal static class BsonArrayExtensions
    {
        internal static object ToDotNetValue(this BsonArray bsonArray)
        {
            return bsonArray
                .Select(BsonTypeMapper.MapToDotNetValue)
                .ToArray();
        }
    }
}