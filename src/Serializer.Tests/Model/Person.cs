using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MongoDB.FrameworkSerializer.Tests.Model
{
    [SerializableAlias("Person")]
    internal class Person : ISerializable
    {
        private readonly List<Address> _addresses = new List<Address>();

        public Person(Email email, FullName fullName)
        {
            Email = email;
            FullName = fullName;
        }

        public Email Email { get; }
        public FullName FullName { get; }
        public IReadOnlyList<Address> Addresses => _addresses.AsReadOnly();

        protected Person(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            Email = info.GetValue<Email>(nameof(Email));
            FullName = info.GetValue<FullName>(nameof(FullName));
            _addresses = info.GetList<Address>(nameof(_addresses)).ToList();
        }

        public void AddAddress(Address address)
        {
            _addresses.Add(address);
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Email), Email);
            info.AddValue(nameof(FullName), FullName);
            info.AddValue(nameof(_addresses), _addresses);
        }
    }
}
