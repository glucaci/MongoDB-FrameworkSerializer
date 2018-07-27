using System;

namespace MongoDB.FrameworkSerializer
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class SerializableAliasAttribute : Attribute
    {
        public string Value { get; }

        public SerializableAliasAttribute(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Alias cannot be empty", nameof(value));
            }

            Value = value;
        }
    }
}