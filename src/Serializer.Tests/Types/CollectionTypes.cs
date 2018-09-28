using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MongoDB.FrameworkSerializer.Tests.Types
{
    [SerializableAlias("CollectionTypes")]
    internal class CollectionTypes : ISerializable
    {
        internal readonly IList<int> ListOfInt;
        internal readonly ICollection<string> CollectionOfString;
        internal readonly IImmutableList<double> ImmutableListOfDouble;
        internal readonly IList<CustomType> ListOfCustom;

        public CollectionTypes()
        {
            ListOfInt = new List<int>(new []{1,3,5});
            CollectionOfString = new Collection<string>(new []{"1", "3", "5"});
            ImmutableListOfDouble = new []{ 1.0, 3.0, 5.0}.ToImmutableList();
            ListOfCustom = new[]
            {
                new CustomType("Custom1", 128.0),
                new CustomType("Custom2", 256.0)
            };
        }

        protected CollectionTypes(SerializationInfo info, StreamingContext context)
        {
            ListOfInt = info.GetList<int>(nameof(ListOfInt));
            CollectionOfString = info.GetList<string>(nameof(CollectionOfString));
            ImmutableListOfDouble = info.GetList<double>(nameof(ImmutableListOfDouble)).ToImmutableList();
            ListOfCustom = info.GetList<CustomType>(nameof(ListOfCustom));
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(ListOfInt), ListOfInt);
            info.AddValue(nameof(CollectionOfString), CollectionOfString);
            info.AddValue(nameof(ImmutableListOfDouble), ImmutableListOfDouble);
            info.AddValue(nameof(ListOfCustom), ListOfCustom);
        }
    }

    [SerializableAlias("CustomType")]
    internal class CustomType : ISerializable
    {
        internal readonly string String;
        internal readonly double Double;

        public CustomType(string _string, double _double)
        {
            String = _string;
            Double = _double;
        }

        protected CustomType(SerializationInfo info, StreamingContext context)
        {
            String = info.GetValue<string>(nameof(String));
            Double = info.GetValue<double>(nameof(Double));
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(String), String);
            info.AddValue(nameof(Double), Double);
        }
    }
}    