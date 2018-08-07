using System;
using System.Reflection;
using System.Runtime.Serialization;
using Xunit;

namespace MongoDB.FrameworkSerializer.Tests
{
    public class SerializableConstructorTests
    {
        [Theory]
        [InlineData(typeof(DefaultConstructor))]
        [InlineData(typeof(PublicConstructor))]
        [InlineData(typeof(ProtectedConstructor))]
        [InlineData(typeof(PrivateConstructor))]
        public void DiscoverSerializableConstructor(Type type)
        {
            ConstructorInfo constructor = type
                .GetSerializableConstructor();

            ParameterInfo[] parameters = constructor.GetParameters();
            Assert.Equal(typeof(SerializationInfo), parameters[0].ParameterType);
            Assert.Equal(typeof(StreamingContext), parameters[1].ParameterType);
        }

        private class DefaultConstructor : ISerializable
        {
            protected DefaultConstructor(
                SerializationInfo info, StreamingContext context)
            {
            }

            void ISerializable.GetObjectData(
                SerializationInfo info, StreamingContext context)
            {
            }
        }

        private class PublicConstructor : ISerializable
        {
            public PublicConstructor(string value)
            {
            }

            protected PublicConstructor(
                SerializationInfo info, StreamingContext context)
            {
            }

            void ISerializable.GetObjectData(
                SerializationInfo info, StreamingContext context)
            {
            }
        }

        private class ProtectedConstructor : ISerializable
        {
            protected ProtectedConstructor(string value)
            {
            }

            protected ProtectedConstructor(
                SerializationInfo info, StreamingContext context)
            {
            }

            void ISerializable.GetObjectData(
                SerializationInfo info, StreamingContext context)
            {
            }
        }

        private class PrivateConstructor : ISerializable
        {
            private PrivateConstructor(string value)
            {
            }

            protected PrivateConstructor(
                SerializationInfo info, StreamingContext context)
            {
            }

            void ISerializable.GetObjectData(
                SerializationInfo info, StreamingContext context)
            {
            }
        }
    }
}
