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

            string result = Serialize(person);

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
            string input =
                "{ " +
                    "\"__typeAlias\" : \"FullName\", " +
                    "\"_firstName\" : \"Foo\", " +
                    "\"_lastName\" : \"Bar\" " +
                "}";

            FullName result = Deserialize<FullName>(input);

            Assert.NotNull(result);
            Assert.Equal("Foo, Bar", (string)result);
        }

        [Fact]
        public void Serialize_TwoLevelDepthObject()
        {
            Person person = new Person(new Email("foo@gmail.com"), new FullName("Foo", "Bar"));

            string result = Serialize(person);

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

            Person result = Deserialize<Person>(input);

            Assert.NotNull(result);
            Assert.Equal(new FullName("Foo", "Bar"), result.FullName);
            Assert.Equal(new Email("foo@gmail.com"), result.Email);
        }
    }
}
