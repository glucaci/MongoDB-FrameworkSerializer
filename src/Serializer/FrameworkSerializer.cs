using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace MongoDB.FrameworkSerializer
{
    internal class FrameworkSerializer<T> : IBsonSerializer<T>
    {
        object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return DeserializeCore(context, args);
        }

        public T Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return (T) DeserializeCore(context, args);
        }

        private ISerializable DeserializeCore(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            IBsonReader reader = context.Reader;
            reader.ReadStartDocument();

            var info = new SerializationInfo(typeof(object), Formatter.Default);
            Type objectType = null;

            BsonType type;
            while(reader.ReadBsonType() != BsonType.EndOfDocument)
            {
                var name = reader.ReadName();
                type = reader.GetCurrentBsonType();
                object value = null;

                if (type == BsonType.Document)
                {
                    value = DeserializeCore(context, args);
                }
                else
                {
                    value = BsonValueSerializer.Instance.Deserialize(context).ToNative();
                }

                if (name == Conventions.TypeAlias)
                {
                    objectType = FrameworkSerializerRegistry.Get(value.ToString());
                    info.SetType(objectType);
                }

                if (name == Conventions.Type)
                {
                    objectType = Type.GetType($"{value},{args.NominalType.Assembly.FullName}");
                    info.SetType(objectType);
                }

                info.AddValue(name, value);
            };

            reader.ReadEndDocument();

            var typeConstructor = objectType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault();
            return typeConstructor.Invoke(new object[] { info, new StreamingContext(StreamingContextStates.Persistence) }) as ISerializable;
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            Serialize(context, args, (T)value);
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, T value)
        {
            context.Writer.WriteStartDocument();
            context.Writer.WriteTypeInformation(value);

            SerializationInfo info = ((ISerializable)value)
                .GetSerializationInfo();

            foreach (SerializationEntry entry in info)
            {
                context.Writer.WriteName(entry.Name);

                if (entry.Value is ISerializable)
                {
                    var serializer = BsonSerializer
                        .SerializerRegistry
                        .GetSerializer(entry.ObjectType);

                    serializer.Serialize(context, args, entry.Value);
                }
                else
                {
                    var bsonValue = BsonValue.Create(entry.Value);
                    BsonValueSerializer
                        .Instance
                        .Serialize(context, bsonValue);
                }
            }

            context.Writer.WriteEndDocument();
        }

        public Type ValueType => typeof(T);
    }
}