using MongoDB.Bson.Serialization;
using MongoDB.FrameworkSerializer.Tests.Models;
using Xunit;

namespace MongoDB.FrameworkSerializer.Tests
{
    public class FrameworkSerializerProviderTests
    {
        [Fact]
        public void RegisterProvider_LookupSerializer_FrameworkSerializerWorks()
        {
            BsonSerializer.RegisterSerializationProvider(FrameworkSerializerProvider.Instance);

            IBsonSerializer<Email> emailSerializer = BsonSerializer.LookupSerializer<Email>();

            Assert.NotNull(emailSerializer);
        }

        [Fact]
        public void RegisterProvider_LookupSerializerTwice_ReturnsCachedSerializer()
        {
            BsonSerializer.RegisterSerializationProvider(FrameworkSerializerProvider.Instance);
            IBsonSerializer<Email> firstSerializer = BsonSerializer.LookupSerializer<Email>();

            IBsonSerializer<Email> secondSerializer = BsonSerializer.LookupSerializer<Email>();

            Assert.Equal(firstSerializer, secondSerializer);
        }
    }
}
