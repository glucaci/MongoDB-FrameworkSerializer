using System;
using System.Runtime.Serialization;

namespace MongoDB.FrameworkSerializer.Tests.Models
{
    internal class Email : ISerializable
    {
        private readonly string _value;

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Email cannot be empty", nameof(value));
            }

            // Some email regex checks

            _value = value;
        }

        protected Email(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            _value = info.GetString(nameof(_value));
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(_value), _value);
        }
    }
}