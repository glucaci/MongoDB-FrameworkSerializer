using System;
using System.Linq;
using System.Reflection;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

namespace MongoDB.FrameworkSerializer
{
    internal static class BsonWriterExtensions
    {
        internal static void WriteTypeInformation<T>(this IBsonWriter writer, T value)
        {
            Type valueType = value.GetType();

            SerializableAliasAttribute serializableAlias =
                (SerializableAliasAttribute)valueType
                    .GetCustomAttributes(typeof(SerializableAliasAttribute))
                    .FirstOrDefault();

            if (serializableAlias != null)
            {
                writer.WriteName(Conventions.TypeAlias);
                writer.WriteString(serializableAlias.Value);
            }
            else
            {
                writer.WriteName(Conventions.Type);
                writer.WriteString(valueType.FullName);
            }
        }

        internal static Type FindDocumentType(this IBsonReader reader, Type nominalType)
        {
            var bookmark = reader.GetBookmark();
            var actualType = nominalType;
            reader.ReadStartDocument();

            if (reader.FindElement(Conventions.Type))
            {
                actualType = ReadActualType(reader, nominalType);
            }

            reader.ReturnToBookmark(bookmark);
            reader.ReadStartDocument();

            if (reader.FindElement(Conventions.TypeAlias))
            {
                actualType = ReadActualType(reader, nominalType);
            }

            reader.ReturnToBookmark(bookmark);
            return actualType;
        }

        private static Type ReadActualType(IBsonReader reader, Type nominalType)
        {
            var context = BsonDeserializationContext.CreateRoot(reader);

            if (ValueSerializer.Deserialize(context.Reader) is string type)
            {
                return LookupType(nominalType, type);
            }

            return nominalType;
        }

        private static Type LookupType(Type nominalType, string typeHint)
        {
            Type type = Type.GetType(typeHint);

            if (type == null)
            {
                type = nominalType.Assembly
                    .GetTypes()
                    .FirstOrDefault(t => t.FullName?.EndsWith(typeHint) ?? false);
            }

            if (type == null)
            {
                type = TypeNameDiscriminator.GetActualType(typeHint);
            }

            if (type == null)
            {
                type = BsonClassMap
                    .GetRegisteredClassMaps()
                    .SelectMany(x => x.AllMemberMaps)
                    .Where(x => x.MemberType.FullName?.EndsWith(typeHint) ?? false)
                    .Select(x => x.MemberType)
                    .FirstOrDefault();
            }

            if (type != null)
            {
                return type;
            }

            throw new InvalidOperationException($"Could not find type for {typeHint}");
        }
    }
}