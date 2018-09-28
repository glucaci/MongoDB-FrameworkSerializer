using System.IO;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

namespace MongoDB.FrameworkSerializer.Tests
{
    public class TestBase
    {
        static TestBase()
        {
            BsonSerializer.RegisterSerializationProvider(FrameworkSerializerProvider.Instance);
        }

        protected string Serialize<T>(T value)
        {
            IBsonSerializer<T> serializer = BsonSerializer.LookupSerializer<T>();

            string result;
            using (var textWriter = new StringWriter())
            using (var writer = new JsonWriter(textWriter))
            {
                var context = BsonSerializationContext.CreateRoot(writer);
                var args = new BsonSerializationArgs { NominalType = typeof(T) };

                serializer.Serialize(context, args, value);
                result = textWriter.ToString();
            }

            return result;
        }

        protected T Deserialize<T>(string input)
        {
            IBsonSerializer<T> serializer = BsonSerializer.LookupSerializer<T>();

            T result;
            using (var textReader = new StringReader(input))
            using (var reader = new JsonReader(textReader))
            {
                var context = BsonDeserializationContext.CreateRoot(reader);
                var args = new BsonDeserializationArgs { NominalType = typeof(T) };

                result = serializer.Deserialize(context, args);
            }

            return result;
        }
    }
}