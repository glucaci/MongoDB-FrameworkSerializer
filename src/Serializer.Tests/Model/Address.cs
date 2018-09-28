using System;
using System.Runtime.Serialization;

namespace MongoDB.FrameworkSerializer.Tests.Model
{
    [SerializableAlias("Address")]
    internal class Address : ISerializable, IEquatable<Address>
    {
        private readonly string _street;
        private readonly int _number;
        private readonly int _cityCode;

        public Address(string street, int number, int cityCode)
        {
            if (string.IsNullOrWhiteSpace(street))
            {
                throw new ArgumentException(
                    "Street cannot be empty", nameof(street));
            }

            if (number <= 0)
            {
                throw new ArgumentException(
                    "Street number cannot be empty", nameof(number));
            }

            if (cityCode <= 0)
            {
                throw new ArgumentException(
                    "City code cannot be empty", nameof(cityCode));
            }

            _street = street;
            _number = number;
            _cityCode = cityCode;
        }

        public static explicit operator string(Address address)
        {
            return $"{address._street} {address._number}, {address._cityCode}";
        }

        protected Address(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            _street = info.GetValue<string>(nameof(_street));
            _number = info.GetValue<int>(nameof(_number));
            _cityCode = info.GetValue<int>(nameof(_cityCode));
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(_street), _street);
            info.AddValue(nameof(_number), _number);
            info.AddValue(nameof(_cityCode), _cityCode);
        }

        public bool Equals(Address other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return String.InvariantEquals(_street, other._street)
                   && _number == other._number
                   && _cityCode == other._cityCode;
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
            return Equals((Address)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = String.GetInvariantHashCode(_street);
                hashCode = hashCode ^ _number.GetHashCode();
                hashCode = hashCode ^ _cityCode.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Address left, Address right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Address left, Address right)
        {
            return !Equals(left, right);
        }
    }
}