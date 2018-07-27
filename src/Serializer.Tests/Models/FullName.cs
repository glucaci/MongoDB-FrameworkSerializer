using System;
using System.Runtime.Serialization;

namespace MongoDB.FrameworkSerializer.Tests.Models
{
    internal class FullName : ISerializable
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
    }
}