using System;
using System.Runtime.Serialization;

namespace MongoDB.FrameworkSerializer.Tests.Models
{
    [SerializableAlias("Person")]
    internal class Person : ISerializable
    {
        public Person(Email email, FullName fullName)
        {
            Email = email;
            FullName = fullName;
        }
        

        public Email Email { get; }
        public FullName FullName { get; }

        protected Person(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            Email = info.GetValue<Email>(nameof(Email));
            FullName = info.GetValue<FullName>(nameof(FullName));
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Email), Email);
            info.AddValue(nameof(FullName), FullName);
        }
    }
}
