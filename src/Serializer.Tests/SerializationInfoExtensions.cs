using System.Runtime.Serialization;

namespace Serializer.Tests
{
    internal static class SerializationInfoExtensions
    {
        internal static T GetValue<T>(this SerializationInfo info, string name)
        {
            return (T)info.GetValue(nameof(name), typeof(T));
        }
    }
}