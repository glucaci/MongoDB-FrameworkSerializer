using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace MongoDB.FrameworkSerializer
{
    internal class TypeBuilder
    {
        private readonly SerializationInfo _serializationInfo;
        private Type _objectType;

        public TypeBuilder()
        {
            _serializationInfo =
                new SerializationInfo(typeof(object), new FormatterConverter());

            _objectType = null;
        }

        public void AddField(string name, object value)
        {
            if (name == Conventions.TypeAlias)
            {
                _objectType = FrameworkSerializerRegistry.Get(value.ToString());
            }

            _serializationInfo.AddValue(name, value);
        }

        public ISerializable Build()
        {
            _serializationInfo.SetType(_objectType);

            var typeConstructor = _objectType.GetConstructors(
                    BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault();

            return typeConstructor.Invoke(new object[]
            {
                _serializationInfo,
                new StreamingContext(StreamingContextStates.Persistence)
            }) as ISerializable;
        }
    }
}