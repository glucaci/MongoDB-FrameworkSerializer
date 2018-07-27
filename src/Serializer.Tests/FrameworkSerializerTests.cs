using System.IO;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.FrameworkSerializer.Tests.Models;
using Xunit;

namespace MongoDB.FrameworkSerializer.Tests
{
    public class FrameworkSerializerTests
    {
        static FrameworkSerializerTests()
        {
            BsonSerializer.RegisterSerializationProvider(FrameworkSerializerProvider.Instance);
        }

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
                var args = new BsonSerializationArgs {NominalType = typeof(Person)};

                serializer.Serialize(context, args, person);
                result = textWriter.ToString();
            }

            Assert.NotNull(result);
            Assert.Equal(
                "{ " +
                    "\"__typeAlias\" : \"MongoDB.FrameworkSerializer.Tests.Models.FullName\"," +
                    " \"_firstName\" : \"Foo\", " +
                    "\"_lastName\" : \"Bar\" " +
                "}", result);
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
                    "\"__typeAlias\" : \"MongoDB.FrameworkSerializer.Tests.Models.Person\", " +
                    "\"Email\" : { " +
                        "\"__typeAlias\" : \"MongoDB.FrameworkSerializer.Tests.Models.Email\", " +
                        "\"_value\" : \"foo@gmail.com\" " +
                    "}, " +
                    "\"FullName\" : { " +
                        "\"__typeAlias\" : \"MongoDB.FrameworkSerializer.Tests.Models.FullName\"," +
                        " \"_firstName\" : \"Foo\", " +
                        "\"_lastName\" : \"Bar\" " +
                    "} " +
                "}", result);
        }
    }
}
