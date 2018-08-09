using System.Runtime.Serialization;
using Xunit;

namespace MongoDB.FrameworkSerializer.Tests
{
    public class SerializableConventionTests : TestBase
    {
        [Fact]
        public void Serialize_ClassWithAttribute()
        {
            WithAttribute withAttribute = new WithAttribute();

            string result = Serialize(withAttribute);

            Assert.NotNull(result);
            Assert.Equal(
                "{ " +
                    "\"__typeAlias\" : \"WithAttribute\" " +
                "}", result);
        }

        [Fact]
        public void Serialize_ClassWithoutAttribute()
        {
            WithoutAttribute withAttribute = new WithoutAttribute();

            string result = Serialize(withAttribute);

            Assert.NotNull(result);
            Assert.Equal(
                "{ " +
                    "\"__type\" : \"MongoDB.FrameworkSerializer.Tests.SerializableConventionTests+WithoutAttribute\" " +
                "}", result);
        }

        [Fact]
        public void Deserialize_ClassWithAttribute()
        {
            string input =
                "{ " +
                    "\"__type\" : \"WithAttribute\" " +
                "}";

            WithAttribute result = Deserialize<WithAttribute>(input);

            Assert.NotNull(result);
        }

        [Fact]
        public void Deserialize_ClassWithoutAttribute()
        {
            string input =
                "{ " +
                    "\"__type\" : \"MongoDB.FrameworkSerializer.Tests.SerializableConventionTests+WithoutAttribute\" " +
                "}";

            WithoutAttribute result = Deserialize<WithoutAttribute>(input);

            Assert.NotNull(result);
        }

        [SerializableAlias("WithAttribute")]
        internal class WithAttribute : ISerializable
        {
            public WithAttribute()
            {
            }

            private WithAttribute(SerializationInfo info, StreamingContext context)
            {
            }

            void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
            {
            }
        }

        private class WithoutAttribute : ISerializable
        {
            public WithoutAttribute()
            {
            }

            private WithoutAttribute(SerializationInfo info, StreamingContext context)
            {
            }

            void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
            {
            }
        }
    }
}
