using Xunit;

namespace MongoDB.FrameworkSerializer.Tests.Model
{
    public class ComplexModelTests : TestBase
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
        public void Serialize_MultiLevelDepthObject()
        {
            Person person = new Person(new Email("foo@gmail.com"), new FullName("Foo", "Bar"));
            person.AddAddress(new Address("One Microsoft Way", 1, 98052));
            person.AddAddress(new Address("148th Ave NE", 5000, 98052));

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
                    "}, " +
                    "\"_addresses\" : [" +
                        "{ " +
                            "\"__typeAlias\" : \"Address\", " +
                            "\"_street\" : \"One Microsoft Way\", " +
                            "\"_number\" : 1, "+
                            "\"_cityCode\" : 98052 "+
                        "}, " +
                        "{ " +
                            "\"__typeAlias\" : \"Address\", "+
                            "\"_street\" : \"148th Ave NE\", " +
                            "\"_number\" : 5000, "+
                            "\"_cityCode\" : 98052 "+
                        "}"+
                    "] "+
                "}", result);
        }

        [Fact]
        public void Deserialize_MultiLevelDepthObject()
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
                    "}, " +
                    "\"_addresses\" : [" +
                        "{ " +
                            "\"__typeAlias\" : \"Address\", " +
                            "\"_street\" : \"One Microsoft Way\", " +
                            "\"_number\" : 1, " +
                            "\"_cityCode\" : 98052 " +
                        "}, " +
                        "{ " +
                            "\"__typeAlias\" : \"Address\", " +
                            "\"_street\" : \"148th Ave NE\", " +
                            "\"_number\" : 5000, " +
                            "\"_cityCode\" : 98052 " +
                        "}" +
                    "] " +
                "}";

            Person result = Deserialize<Person>(input);

            Assert.NotNull(result);
            Assert.Equal(new FullName("Foo", "Bar"), result.FullName);
            Assert.Equal(new Email("foo@gmail.com"), result.Email);
            Assert.Collection(result.Addresses,
                address => Assert.Equal(new Address("One Microsoft Way", 1, 98052), address),
                address => Assert.Equal(new Address("148th Ave NE", 5000, 98052), address));
        }
    }
}
