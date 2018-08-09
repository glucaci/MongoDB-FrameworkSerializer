using System.IO;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.FrameworkSerializer.Tests.Models;
using Xunit;

namespace MongoDB.FrameworkSerializer.Tests
{
    public class FrameworkSerializerTests : TestBase
    {
        [Fact]
        public void Serialize_OneLevelDepthObject()
        {
            FullName person = new FullName("Foo", "Bar");
            IBsonSerializer<FullName> serializer = BsonSerializer.LookupSerializer<FullName>();

            string result;
            using (var textWriter = new StringWriter())
            using (var writer = new JsonWriter(textWriter))
            {
                var context = BsonSerializationContext.CreateRoot(writer);
                var args = new BsonSerializationArgs {NominalType = typeof(FullName)};

                serializer.Serialize(context, args, person);
                result = textWriter.ToString();
            }

            Assert.NotNull(result);
            Assert.Equal(
                "{ " +
                    "\"__typeAlias\" : \"FullName\", " +
                    "\"_firstName\" : \"Foo\", " +
                    "\"_lastName\" : \"Bar\" " +
                "}", result);
        }

        [Fact]
        public void Deserialize_OneLevelDepthObject()
        {
            IBsonSerializer<FullName> serializer = BsonSerializer.LookupSerializer<FullName>();
            string input =
                "{ " +
                    "\"__typeAlias\" : \"FullName\", " +
                    "\"_firstName\" : \"Foo\", " +
                    "\"_lastName\" : \"Bar\" " +
                "}";

            FullName result;
            using (var textReader = new StringReader(input))
            using (var reader = new JsonReader(textReader))
            {
                var context = BsonDeserializationContext.CreateRoot(reader);
                var args = new BsonDeserializationArgs { NominalType = typeof(FullName) };

                result = serializer.Deserialize(context, args);
            }

            Assert.NotNull(result);
            Assert.Equal("Foo, Bar", (string)result);
        }

        [Fact]
        public void Serialize_TwoLevelDepthObject()
        {
            Person person = new Person(new Email("foo@gmail.com"), new FullName("Foo", "Bar"));
            IBsonSerializer<Person> serializer = BsonSerializer.LookupSerializer<Person>();

            string result;
            using (var textWriter = new StringWriter())
            using (var writer = new JsonWriter(textWriter))
            {
                var context = BsonSerializationContext.CreateRoot(writer);
                var args = new BsonSerializationArgs { NominalType = typeof(Person) };

                serializer.Serialize(context, args, person);
                result = textWriter.ToString();
            }

            Assert.NotNull(result);
            Assert.Equal(
                "{ " +
                    "\"__typeAlias\" : \"Person\", " +
                    "\"Email\" : { " +
                        "\"__typeAlias\" : \"Email\", " +
                        "\"_value\" : \"foo@gmail.com\" " +
                    "}, " +
                    "\"FullName\" : { " +
                        "\"__typeAlias\" : \"FullName\", " +
                        "\"_firstName\" : \"Foo\", " +
                        "\"_lastName\" : \"Bar\" " +
                    "} " +
                "}", result);
        }

        [Fact]
        public void Deserialize_TwoLevelDepthObject()
        {
            IBsonSerializer<Person> serializer = BsonSerializer.LookupSerializer<Person>();
            string input =
                "{ " +
                    "\"__typeAlias\" : \"Person\", " +
                    "\"Email\" : { " +
                        "\"__typeAlias\" : \"Email\", " +
                        "\"_value\" : \"foo@gmail.com\" " +
                    "}, " +
                    "\"FullName\" : { " +
                        "\"__typeAlias\" : \"FullName\", " +
                        "\"_firstName\" : \"Foo\", " +
                        "\"_lastName\" : \"Bar\" " +
                    "} " +
                "}";

            Person result;
            using (var textReader = new StringReader(input))
            using (var reader = new JsonReader(textReader))
            {
                var context = BsonDeserializationContext.CreateRoot(reader);
                var args = new BsonDeserializationArgs { NominalType = typeof(Person) };

                result = serializer.Deserialize(context, args);
            }

            Assert.NotNull(result);
            Assert.Equal(new FullName("Foo", "Bar"), result.FullName);
            Assert.Equal(new Email("foo@gmail.com"), result.Email);
        }
    }
}
