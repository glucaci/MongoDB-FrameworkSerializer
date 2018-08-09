using System;
using System.Runtime.Serialization;

namespace MongoDB.FrameworkSerializer
{
    internal class TypeBuilder
    {
        private static readonly StreamingContext SteamingContext =
            new StreamingContext(StreamingContextStates.Persistence);

        private readonly SerializationInfo _serializationInfo;
        private Type _objectType;

        public TypeBuilder()
        {
            _serializationInfo = new SerializationInfo(
                typeof(object), new FormatterConverter());

            _objectType = null;
        }

        public void AddField(string name, object value)
        {
            if (name == Conventions.TypeAlias
                || name == Conventions.Type)
            {
                _objectType = FrameworkSerializerRegistry
                    .Get(value.ToString());
            }

            _serializationInfo.AddValue(name, value);
        }

        public ISerializable Build()
        {
            _serializationInfo.SetType(_objectType);

            return _objectType
                .GetSerializableConstructor()
                .Invoke(new object[]
                {
                    _serializationInfo,
                    SteamingContext
                }) as ISerializable;
        }
    }
}