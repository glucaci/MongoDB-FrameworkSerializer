using System.IO;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.FrameworkSerializer.Tests.Models;

namespace MongoDB.FrameworkSerializer.Tests
{
    public class TestBase
    {
        static TestBase()
        {
            BsonSerializer.RegisterSerializationProvider(FrameworkSerializerProvider.Instance);
            FrameworkSerializerRegistry.Map("NativeTypes", typeof(NativeTypes));
            FrameworkSerializerRegistry.Map("WithAttribute", typeof(SerializableConventionTests.WithAttribute));
            FrameworkSerializerRegistry.Map("Email", typeof(Email));
            FrameworkSerializerRegistry.Map("FullName", typeof(FullName));
            FrameworkSerializerRegistry.Map("Person", typeof(Person));
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
                var args = new BsonDeserializationArgs { NominalType = typeof(NativeTypes) };

                result = serializer.Deserialize(context, args);
            }

            return result;
        }
    }
}