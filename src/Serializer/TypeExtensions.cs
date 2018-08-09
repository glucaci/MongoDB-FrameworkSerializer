using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace MongoDB.FrameworkSerializer
{
    internal static class TypeExtensions
    {
        private static readonly Type SerializationInfoType =
            typeof(SerializationInfo);

        private static readonly Type StreamingContextType =
            typeof(StreamingContext);

        internal static ConstructorInfo GetSerializableConstructor(this Type type)
        {
            ConstructorInfo constructorInfo = type
                .GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(info =>
                {
                    ParameterInfo[] parameters = info.GetParameters();
                    if (parameters.Length == 2)
                    {
                        return parameters[0].ParameterType == SerializationInfoType
                               && parameters[1].ParameterType == StreamingContextType;
                    }

                    return false;
                });

            if (constructorInfo == null)
            {
                throw new InvalidOperationException(
                    $"Type \"{type.FullName}\" has no Serialization constructor.");
            }

            return constructorInfo;
        }
    }
}