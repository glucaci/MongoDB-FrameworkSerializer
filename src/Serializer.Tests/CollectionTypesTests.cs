using MongoDB.FrameworkSerializer.Tests.Models;
using Xunit;

namespace MongoDB.FrameworkSerializer.Tests
{
    public class CollectionTypesTests : TestBase
    {
        [Fact]
        public void Serialize_Collections()
        {
            CollectionTypes collectionTypes = new CollectionTypes();

            string result = Serialize(collectionTypes);

            Assert.NotNull(result);
            Assert.Equal(
                "{ " +
                    "\"__typeAlias\" : \"CollectionTypes\", " +
                    "\"ListOfInt\" : [" +
                        "1, " +
                        "3, " +
                        "5" +
                    "], " +
                    "\"CollectionOfString\" : [" +
                        "\"1\", " +
                        "\"3\", " +
                        "\"5\"" +
                    "], " +
                    "\"ImmutableListOfDouble\" : [" +
                        "1.0, " +
                        "3.0, " +
                        "5.0" +
                    "] " +
                "}", result);
        }

        [Fact]
        public void Deserialize_Collections()
        {
            string input =
                "{ " +
                    "\"__typeAlias\" : \"CollectionTypes\", " +
                    "\"ListOfInt\" : [" +
                        "1, " +
                        "3, " +
                        "5" +
                    "], " +
                    "\"CollectionOfString\" : [" +
                        "\"1\", " +
                        "\"3\", " +
                        "\"5\"" +
                    "], " +
                    "\"ImmutableListOfDouble\" : [" +
                        "1.0, " +
                        "3.0, " +
                        "5.0" +
                    "] " +
                "}";

            CollectionTypes result = Deserialize<CollectionTypes>(input);

            Assert.NotNull(result);
            Assert.Equal(new [] { 1, 3, 5 }, result.ListOfInt);
            Assert.Equal(new [] { "1", "3", "5" }, result.CollectionOfString);
            Assert.Equal(new [] { 1.0, 3.0, 5.0 }, result.ImmutableListOfDouble);
        }
    }
}