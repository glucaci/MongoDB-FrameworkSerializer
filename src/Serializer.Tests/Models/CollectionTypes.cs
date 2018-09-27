using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MongoDB.FrameworkSerializer.Tests.Models
{
    [SerializableAlias("CollectionTypes")]
    internal class CollectionTypes : ISerializable
    {
        internal readonly IList<int> ListOfInt;
        internal readonly ICollection<string> CollectionOfString;
        internal readonly IImmutableList<double> ImmutableListOfDouble;

        public CollectionTypes()
        {
            ListOfInt = new List<int>(new []{1,3,5});
            CollectionOfString = new Collection<string>(new []{"1", "3", "5"});
            ImmutableListOfDouble = new []{ 1.0, 3.0, 5.0}.ToImmutableList();
        }

        protected CollectionTypes(SerializationInfo info, StreamingContext context)
        {
            ListOfInt = info.GetList<int>(nameof(ListOfInt));
            CollectionOfString = info.GetList<string>(nameof(CollectionOfString));
            ImmutableListOfDouble = info.GetList<double>(nameof(ImmutableListOfDouble)).ToImmutableList();
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(ListOfInt), ListOfInt);
            info.AddValue(nameof(CollectionOfString), CollectionOfString);
            info.AddValue(nameof(ImmutableListOfDouble), ImmutableListOfDouble);
        }
    }
}