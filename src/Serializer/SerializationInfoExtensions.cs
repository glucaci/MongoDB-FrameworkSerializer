using System;
using System.Runtime.Serialization;

namespace MongoDB.FrameworkSerializer
{
    public static class SerializationInfoExtensions
    {
        public static T GetValue<T>(this SerializationInfo info, string name)
        {
            return (T)info.GetValue(name, typeof(T));
        }

        public static T[] GetList<T>(this SerializationInfo info, string name)
        {
            var array = info.GetValue<object[]>(name);
            var typedArray = new T[array.Length];
            Array.Copy(array, typedArray, array.Length);

            return typedArray;
        }

        internal static SerializationInfo GetSerializationInfo(this ISerializable serializable)
        {
            var serializationInfo = new SerializationInfo(serializable.GetType(), new FormatterConverter());
            serializable.GetObjectData(serializationInfo, new StreamingContext(StreamingContextStates.Persistence));

            return serializationInfo;
        }
    }
}