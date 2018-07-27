using System;
using System.Linq;
using System.Reflection;
using MongoDB.Bson.IO;

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
    }
}