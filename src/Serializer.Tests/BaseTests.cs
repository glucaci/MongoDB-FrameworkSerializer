using MongoDB.Bson.Serialization;
using MongoDB.FrameworkSerializer.Tests.Models;

namespace MongoDB.FrameworkSerializer.Tests
{
    public class BaseTests
    {
        static BaseTests()
        {
            BsonSerializer.RegisterSerializationProvider(FrameworkSerializerProvider.Instance);
            FrameworkSerializerRegistry.Map("NativeTypes", typeof(NativeTypes));
            FrameworkSerializerRegistry.Map("Email", typeof(Email));
            FrameworkSerializerRegistry.Map("FullName", typeof(FullName));
            FrameworkSerializerRegistry.Map("Person", typeof(Person));
        }
    }
}