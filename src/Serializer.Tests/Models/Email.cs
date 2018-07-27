using System;
using System.Runtime.Serialization;

namespace MongoDB.FrameworkSerializer.Tests.Models
{
    internal class Email : ISerializable, IEquatable<Email>
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

        public bool Equals(Email other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return String.InvariantEquals(_value, other._value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((Email) obj);
        }

        public override int GetHashCode()
        {
            return String.GetInvariantHashCode(_value);
        }

        public static bool operator ==(Email left, Email right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Email left, Email right)
        {
            return !Equals(left, right);
        }
    }
}