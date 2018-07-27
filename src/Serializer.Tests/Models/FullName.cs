using System;
using System.Runtime.Serialization;

namespace MongoDB.FrameworkSerializer.Tests.Models
{
    internal class FullName : ISerializable, IEquatable<FullName>
    {
        private readonly string _firstName;
        private readonly string _lastName;

        public FullName(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException(
                    "Fist Name cannot be empty", nameof(firstName));
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException(
                    "Last Name cannot be empty", nameof(lastName));
            }

            _firstName = firstName;
            _lastName = lastName;
        }

        public static explicit operator string(FullName fullName)
        {
            return $"{fullName._firstName}, {fullName._lastName}";
        }

        protected FullName(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            _firstName = info.GetString(nameof(_firstName));
            _lastName = info.GetString(nameof(_lastName));
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(_firstName), _firstName);
            info.AddValue(nameof(_lastName), _lastName);
        }

        public bool Equals(FullName other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return String.InvariantEquals(_firstName, other._firstName)
                   && String.InvariantEquals(_lastName, other._lastName);
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
            return Equals((FullName) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = String.GetInvariantHashCode(_firstName);
                hashCode = hashCode ^ String.GetInvariantHashCode(_lastName);
                return hashCode;
            }
        }

        public static bool operator ==(FullName left, FullName right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FullName left, FullName right)
        {
            return !Equals(left, right);
        }
    }
}