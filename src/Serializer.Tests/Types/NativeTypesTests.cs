﻿using System;
using MongoDB.FrameworkSerializer.Tests.Types;
using Xunit;

namespace MongoDB.FrameworkSerializer.Tests
{
    public class NativeTypesTests : TestBase
    {
        [Fact]
        public void Serialize_MaxValues()
        {
            NativeTypes nativeTypes = new NativeTypes(withMaxValues: true);

            string result = Serialize(nativeTypes);

            Assert.NotNull(result);
            Assert.Equal(
                "{ " +
                    "\"__typeAlias\" : \"NativeTypes\", " +
                    "\"Bool\" : true, " +
                    "\"Byte\" : 255, " +
                    "\"Sbyte\" : 127, " +
                    "\"Char\" : 65535, " +
                    "\"Decimal\" : \"79,228,162,514,264,337,593,543,950,335.00\", " +
                    "\"Double\" : 1.7976931348623157E+308, " +
                    "\"Float\" : 3.4028234663852886E+38, " +
                    "\"Int\" : 2147483647, " +
                    "\"Uint\" : NumberLong(\"4294967295\"), " +
                    "\"Long\" : NumberLong(\"9223372036854775807\"), " +
                    "\"Ulong\" : \"18446744073709551615\", " +
                    "\"Short\" : 32767, " +
                    "\"Ushort\" : 65535, " +
                    "\"String\" : \"Foo\", " +
                    "\"DateTime\" : { " +
                        "\"__type\" : \"System.DateTime\", " +
                        "\"ticks\" : NumberLong(\"3155378975999999999\"), " +
                        "\"dateData\" : \"3155378975999999999\" " +
                    "}, " +
                    "\"Guid\" : \"00000000-0000-0000-0000-000000000000\", " +
                    "\"Null\" : null " +
                "}", result);
        }

        [Fact]
        public void Serialize_MinValues()
        {
            NativeTypes nativeTypes = new NativeTypes(withMaxValues: false);

            string result = Serialize(nativeTypes);

            Assert.NotNull(result);
            Assert.Equal(
                "{ " +
                    "\"__typeAlias\" : \"NativeTypes\", " +
                    "\"Bool\" : false, " +
                    "\"Byte\" : 0, " +
                    "\"Sbyte\" : -128, " +
                    "\"Char\" : 0, " +
                    "\"Decimal\" : \"-79,228,162,514,264,337,593,543,950,335.00\", " +
                    "\"Double\" : -1.7976931348623157E+308, " +
                    "\"Float\" : -3.4028234663852886E+38, " +
                    "\"Int\" : -2147483648, " +
                    "\"Uint\" : NumberLong(0), " +
                    "\"Long\" : NumberLong(\"-9223372036854775808\"), " +
                    "\"Ulong\" : \"0\", " +
                    "\"Short\" : -32768, " +
                    "\"Ushort\" : 0, " +
                    "\"String\" : \"Foo\", " +
                    "\"DateTime\" : { " +
                        "\"__type\" : \"System.DateTime\", " +
                        "\"ticks\" : NumberLong(0), " +
                        "\"dateData\" : \"0\" " +
                    "}, " +
                    "\"Guid\" : \"00000000-0000-0000-0000-000000000000\", " +
                    "\"Null\" : null " +
                "}", result);
        }

        [Fact]
        public void Deserialize_MaxValues()
        {
            string input =
                "{ " +
                    "\"__typeAlias\" : \"NativeTypes\", " +
                    "\"Bool\" : true, " +
                    "\"Byte\" : 255, " +
                    "\"Sbyte\" : 127, " +
                    "\"Char\" : 65535, " +
                    "\"Decimal\" : \"79,228,162,514,264,337,593,543,950,335.00\", " +
                    "\"Double\" : 1.7976931348623157E+308, " +
                    "\"Float\" : 3.4028234663852886E+38, " +
                    "\"Int\" : 2147483647, " +
                    "\"Uint\" : NumberLong(\"4294967295\"), " +
                    "\"Long\" : NumberLong(\"9223372036854775807\"), " +
                    "\"Ulong\" : \"18446744073709551615\", " +
                    "\"Short\" : 32767, " +
                    "\"Ushort\" : 65535, " +
                    "\"String\" : \"Foo\", " +
                    "\"DateTime\" : { " +
                        "\"__type\" : \"System.DateTime\", " +
                        "\"ticks\" : NumberLong(\"3155378975999999999\"), " +
                        "\"dateData\" : \"3155378975999999999\" " +
                    "}, " +
                    "\"Guid\" : \"00000000-0000-0000-0000-000000000000\", " +
                    "\"Null\" : null " +
                "}";

            NativeTypes result = Deserialize<NativeTypes>(input);

            Assert.NotNull(result);
            Assert.True(result.Bool);
            Assert.Equal(byte.MaxValue, result.Byte);
            Assert.Equal(sbyte.MaxValue, result.Sbyte);
            Assert.Equal(char.MaxValue, result.Char);
            Assert.Equal(decimal.MaxValue, result.Decimal);
            Assert.Equal(double.MaxValue, result.Double);
            Assert.Equal(float.MaxValue, result.Float);
            Assert.Equal(int.MaxValue, result.Int);
            Assert.Equal(uint.MaxValue, result.Uint);
            Assert.Equal(long.MaxValue, result.Long);
            Assert.Equal(ulong.MaxValue, result.Ulong);
            Assert.Equal(short.MaxValue, result.Short);
            Assert.Equal(ushort.MaxValue, result.Ushort);
            Assert.Equal("Foo", result.String);
            Assert.Equal(DateTime.MaxValue, result.DateTime);
            Assert.Equal(Guid.Empty, result.Guid);
            Assert.Null(result.Null);
        }

        [Fact]
        public void Deserialize_MinValues()
        {
            string input =
                "{ " +
                    "\"__typeAlias\" : \"NativeTypes\", " +
                    "\"Bool\" : false, " +
                    "\"Byte\" : 0, " +
                    "\"Sbyte\" : -128, " +
                    "\"Char\" : 0, " +
                    "\"Decimal\" : \"-79,228,162,514,264,337,593,543,950,335.00\", " +
                    "\"Double\" : -1.7976931348623157E+308, " +
                    "\"Float\" : -3.4028234663852886E+38, " +
                    "\"Int\" : -2147483648, " +
                    "\"Uint\" : NumberLong(0), " +
                    "\"Long\" : NumberLong(\"-9223372036854775808\"), " +
                    "\"Ulong\" : NumberLong(0), " +
                    "\"Short\" : -32768, " +
                    "\"Ushort\" : 0, " +
                    "\"String\" : \"Foo\", " +
                    "\"DateTime\" : { " +
                        "\"__type\" : \"System.DateTime\", " +
                        "\"ticks\" : NumberLong(0), " +
                        "\"dateData\" : \"0\" " +
                    "}, " +
                    "\"Guid\" : \"00000000-0000-0000-0000-000000000000\", " +
                    "\"Null\" : null " +
                "}";

            NativeTypes result = Deserialize<NativeTypes>(input);

            Assert.NotNull(result);
            Assert.False(result.Bool);
            Assert.Equal(byte.MinValue, result.Byte);
            Assert.Equal(sbyte.MinValue, result.Sbyte);
            Assert.Equal(char.MinValue, result.Char);
            Assert.Equal(decimal.MinValue, result.Decimal);
            Assert.Equal(double.MinValue, result.Double);
            Assert.Equal(float.MinValue, result.Float);
            Assert.Equal(int.MinValue, result.Int);
            Assert.Equal(uint.MinValue, result.Uint);
            Assert.Equal(long.MinValue, result.Long);
            Assert.Equal(ulong.MinValue, result.Ulong);
            Assert.Equal(short.MinValue, result.Short);
            Assert.Equal(ushort.MinValue, result.Ushort);
            Assert.Equal("Foo", result.String);
            Assert.Equal(DateTime.MinValue, result.DateTime);
            Assert.Equal(Guid.Empty, result.Guid);
            Assert.Null(result.Null);
        }
    }
}
