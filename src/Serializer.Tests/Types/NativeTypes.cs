using System;
using System.Runtime.Serialization;

namespace MongoDB.FrameworkSerializer.Tests.Types
{
    [SerializableAlias("NativeTypes")]
    internal class NativeTypes : ISerializable
    {
        internal readonly bool Bool;
        internal readonly byte Byte;
        internal readonly sbyte Sbyte;
        internal readonly char Char;
        internal readonly decimal Decimal;
        internal readonly double Double;
        internal readonly float Float;
        internal readonly int Int;
        internal readonly uint Uint;
        internal readonly long Long;
        internal readonly ulong Ulong;
        internal readonly short Short;
        internal readonly ushort Ushort;
        internal readonly string String;
        internal readonly DateTime DateTime;
        internal readonly Guid Guid;
        internal readonly object Null;

        public NativeTypes(bool withMaxValues)
        {
            Bool = withMaxValues ? true : false;
            Byte = withMaxValues ? byte.MaxValue : byte.MinValue;
            Sbyte = withMaxValues ? sbyte.MaxValue : sbyte.MinValue;
            Char = withMaxValues ? char.MaxValue : char.MinValue;
            Decimal = withMaxValues ? decimal.MaxValue : decimal.MinValue;
            Double = withMaxValues ? double.MaxValue : double.MinValue;
            Float = withMaxValues ? float.MaxValue : float.MinValue;
            Int = withMaxValues ? int.MaxValue : int.MinValue;
            Uint = withMaxValues ? uint.MaxValue : uint.MinValue;
            Long = withMaxValues ? long.MaxValue : long.MinValue;
            Ulong = withMaxValues ? ulong.MaxValue : ulong.MinValue;
            Short = withMaxValues ? short.MaxValue : short.MinValue;
            Ushort = withMaxValues ? ushort.MaxValue : ushort.MinValue;
            String = "Foo";
            DateTime = withMaxValues ? DateTime.MaxValue : DateTime.MinValue;
            Guid = Guid.Empty;
            Null = null;
        }

        protected NativeTypes(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            Bool = info.GetBoolean(nameof(Bool));
            Byte = info.GetByte(nameof(Byte));
            Sbyte = info.GetSByte(nameof(Sbyte));
            Char = info.GetChar(nameof(Char));
            Decimal = info.GetDecimal(nameof(Decimal));
            Double = info.GetDouble(nameof(Double));
            Float = info.GetSingle(nameof(Float));
            Int = info.GetInt32(nameof(Int));
            Uint = info.GetUInt32(nameof(Uint));
            Long = info.GetInt64(nameof(Long));
            Ulong = info.GetUInt64(nameof(Ulong));
            Short = info.GetInt16(nameof(Short));
            Ushort = info.GetUInt16(nameof(Ushort));
            String = info.GetString(nameof(String));
            DateTime = info.GetDateTime(nameof(DateTime));
            Guid = Guid.Parse(info.GetString(nameof(Guid)));
            Null = info.GetValue(nameof(Null), typeof(object));
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Bool), Bool);
            info.AddValue(nameof(Byte), Byte);
            info.AddValue(nameof(Sbyte), Sbyte);
            info.AddValue(nameof(Char), Char);
            info.AddValue(nameof(Decimal), Decimal);
            info.AddValue(nameof(Double), Double);
            info.AddValue(nameof(Float), Float);
            info.AddValue(nameof(Int), Int);
            info.AddValue(nameof(Uint), Uint);
            info.AddValue(nameof(Long), Long);
            info.AddValue(nameof(Ulong), Ulong);
            info.AddValue(nameof(Short), Short);
            info.AddValue(nameof(Ushort), Ushort);
            info.AddValue(nameof(String), String);
            info.AddValue(nameof(DateTime), DateTime);
            info.AddValue(nameof(Guid), Guid);
            info.AddValue(nameof(Null), Null);
        }
    }
}