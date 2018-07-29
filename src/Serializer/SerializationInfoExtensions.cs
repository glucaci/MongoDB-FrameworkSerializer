using System.Runtime.Serialization;

namespace MongoDB.FrameworkSerializer
{
    public static class SerializationInfoExtensions
    {
        public static T GetValue<T>(this SerializationInfo info, string name)
        {
            return (T)info.GetValue(name, typeof(T));
        }

        internal static SerializationInfo GetSerializationInfo(this ISerializable serializable)
        {
            var serializationInfo = new SerializationInfo(serializable.GetType(), new FormatterConverter());
            serializable.GetObjectData(serializationInfo, new StreamingContext(StreamingContextStates.Persistence));

            return serializationInfo;
        }
    }
}