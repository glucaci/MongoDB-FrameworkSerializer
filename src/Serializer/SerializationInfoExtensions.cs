using System.Runtime.Serialization;

namespace MongoDB.FrameworkSerializer
{
    public static class SerializationInfoExtensions
    {
        public static T GetValue<T>(this SerializationInfo info, string name)
        {
            return (T)info.GetValue(nameof(name), typeof(T));
        }

        internal static SerializationInfo GetSerializationInfo(this ISerializable serializable)
        {
            var serializationInfo = new SerializationInfo(serializable.GetType(), Formatter.Default);
            serializable.GetObjectData(serializationInfo, new StreamingContext(StreamingContextStates.Persistence));

            return serializationInfo;
        }
    }
}