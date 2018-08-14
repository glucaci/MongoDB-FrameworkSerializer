using System.Runtime.Serialization;

namespace MongoDB.FrameworkSerializer
{
    internal static class SerializationContext
    {
        internal static readonly StreamingContext Instance =
            new StreamingContext(StreamingContextStates.Persistence);
    }
}