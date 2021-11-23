using System;
using System.Data;
using BassUtils.MsSql;
using Microsoft.Data.SqlClient.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BassUtils.UnitTests.MsSql
{
    [TestClass]
    public class SqlDataRecordExtensionsTests
    {
        private static readonly SqlMetaData[] RecordMetaData =
        {
                new SqlMetaData("TheString", SqlDbType.VarChar, 100),
                new SqlMetaData("TheDateTime", SqlDbType.DateTime),
                new SqlMetaData("TheDateTimeOffset", SqlDbType.DateTimeOffset),
                new SqlMetaData("TheTimeSpan", SqlDbType.Time),
                new SqlMetaData("TheByte", SqlDbType.TinyInt),
                new SqlMetaData("TheShort", SqlDbType.SmallInt),
                new SqlMetaData("TheInt", SqlDbType.Int),
                new SqlMetaData("TheLong", SqlDbType.BigInt),
                new SqlMetaData("TheDecimal", SqlDbType.Decimal),
                new SqlMetaData("TheBool", SqlDbType.Bit),
                new SqlMetaData("TheChar", SqlDbType.VarChar, 1),       // Not used, SqlDataRecord does not support chars, it throws an exception!
                new SqlMetaData("TheFloat", SqlDbType.Real),
                new SqlMetaData("TheDouble", SqlDbType.Float),
                new SqlMetaData("TheGuid", SqlDbType.UniqueIdentifier),
            };

        private SqlDataRecord TheRecord;

        [TestInitialize]
        public void Init()
        {
            TheRecord = new SqlDataRecord(RecordMetaData);
        }

        [TestMethod]
        public void StringExtensions()
        {
            var str = "Hello world!";
            TheRecord.SetNullableString(0, str);
            Assert.AreEqual(TheRecord.GetString(0), str);

            str = "Goodbye, cruel world...";
            TheRecord.SetNullableString("TheString", str);
            Assert.AreEqual(TheRecord.GetString("TheString"), str);

            TheRecord.SetNullableString(0, null);
            Assert.IsNull(TheRecord.GetNullableString(0));

            TheRecord.SetNullableString("TheString", null);
            Assert.IsNull(TheRecord.GetNullableString("TheString"));
        }

        [TestMethod]
        public void DateTimeExtensions()
        {
            var dt = new DateTime(1970, 2, 3);
            TheRecord.SetNullableDateTime(1, dt);
            Assert.AreEqual(TheRecord.GetDateTime(1), dt);

            dt = new DateTime(1970, 11, 4);
            TheRecord.SetNullableDateTime("TheDateTime", dt);
            Assert.AreEqual(TheRecord.GetDateTime("TheDateTime"), dt);

            TheRecord.SetNullableDateTime(1, null);
            Assert.IsNull(TheRecord.GetNullableDateTime(1));

            TheRecord.SetNullableDateTime("TheDateTime", null);
            Assert.IsNull(TheRecord.GetNullableDateTime("TheDateTime"));
        }

        [TestMethod]
        public void DateTimeOffsetExtensions()
        {
            var dto = new DateTimeOffset(1970, 2, 3, 12, 13, 14, TimeSpan.FromHours(-2));
            TheRecord.SetNullableDateTimeOffset(2, dto);
            Assert.AreEqual(TheRecord.GetDateTimeOffset(2), dto);

            dto = new DateTimeOffset(1980, 2, 3, 12, 13, 14, TimeSpan.FromHours(2));
            TheRecord.SetNullableDateTimeOffset("TheDateTimeOffset", dto);
            Assert.AreEqual(TheRecord.GetDateTimeOffset("TheDateTimeOffset"), dto);

            TheRecord.SetNullableDateTimeOffset(2, null);
            Assert.IsNull(TheRecord.GetNullableDateTimeOffset(2));

            TheRecord.SetNullableDateTimeOffset("TheDateTimeOffset", null);
            Assert.IsNull(TheRecord.GetNullableDateTimeOffset("TheDateTimeOffset"));
        }

        [TestMethod]
        public void TimeSpanExtensions()
        {
            var ts = TimeSpan.FromMilliseconds(12242746);
            TheRecord.SetNullableTimeSpan(3, ts);
            Assert.AreEqual(TheRecord.GetTimeSpan(3), ts);

            ts = TimeSpan.FromMilliseconds(38764);
            TheRecord.SetNullableTimeSpan("TheTimeSpan", ts);
            Assert.AreEqual(TheRecord.GetTimeSpan("TheTimeSpan"), ts);

            TheRecord.SetNullableTimeSpan(3, null);
            Assert.IsNull(TheRecord.GetNullableTimeSpan(3));

            TheRecord.SetNullableTimeSpan("TheTimeSpan", null);
            Assert.IsNull(TheRecord.GetNullableTimeSpan("TheTimeSpan"));
        }

        [TestMethod]
        public void ByteExtensions()
        {
            var b = byte.MinValue;
            TheRecord.SetNullableByte(4, b);
            Assert.AreEqual(TheRecord.GetByte(4), b);

            b = byte.MaxValue;
            TheRecord.SetNullableByte("TheByte", b);
            Assert.AreEqual(TheRecord.GetByte("TheByte"), b);

            TheRecord.SetNullableByte(4, null);
            Assert.IsNull(TheRecord.GetNullableByte(4));

            TheRecord.SetNullableByte("TheByte", null);
            Assert.IsNull(TheRecord.GetNullableByte("TheByte"));
        }

        [TestMethod]
        public void ShortExtensions()
        {
            var shrt = short.MaxValue;
            TheRecord.SetNullableInt16(5, shrt);
            Assert.AreEqual(TheRecord.GetInt16(5), shrt);

            shrt = short.MinValue;
            TheRecord.SetNullableInt16("TheShort", shrt);
            Assert.AreEqual(TheRecord.GetInt16("TheShort"), shrt);

            TheRecord.SetNullableInt16(5, null);
            Assert.IsNull(TheRecord.GetNullableInt16(5));

            TheRecord.SetNullableInt16("TheShort", null);
            Assert.IsNull(TheRecord.GetNullableInt16("TheShort"));
        }

        [TestMethod]
        public void IntExtensions()
        {
            var i = int.MaxValue;
            TheRecord.SetNullableInt32(6, i);
            Assert.AreEqual(TheRecord.GetInt32(6), i);

            i = int.MinValue;
            TheRecord.SetNullableInt32("TheInt", i);
            Assert.AreEqual(TheRecord.GetInt32("TheInt"), i);

            TheRecord.SetNullableInt32(6, null);
            Assert.IsNull(TheRecord.GetNullableInt32(6));

            TheRecord.SetNullableInt32("TheInt", null);
            Assert.IsNull(TheRecord.GetNullableInt32("TheInt"));
        }

        [TestMethod]
        public void LongExtensions()
        {
            var n = long.MaxValue;
            TheRecord.SetNullableInt64(7, n);
            Assert.AreEqual(TheRecord.GetInt64(7), n);

            n = long.MinValue;
            TheRecord.SetNullableInt64("TheLong", n);
            Assert.AreEqual(TheRecord.GetInt64("TheLong"), n);

            TheRecord.SetNullableInt64(7, null);
            Assert.IsNull(TheRecord.GetNullableInt64(7));

            TheRecord.SetNullableInt64("TheLong", null);
            Assert.IsNull(TheRecord.GetNullableInt64("TheLong"));
        }

        [TestMethod]
        public void DecimalExtensions()
        {
            var n = decimal.MaxValue;
            TheRecord.SetNullableDecimal(8, n);
            Assert.AreEqual(TheRecord.GetDecimal(8), n);

            n = decimal.MinValue;
            TheRecord.SetNullableDecimal("TheDecimal", n);
            Assert.AreEqual(TheRecord.GetDecimal("TheDecimal"), n);

            TheRecord.SetNullableDecimal(8, null);
            Assert.IsNull(TheRecord.GetNullableDecimal(8));

            TheRecord.SetNullableDecimal("TheDecimal", null);
            Assert.IsNull(TheRecord.GetNullableDecimal("TheDecimal"));
        }

        [TestMethod]
        public void BoolExtensions()
        {
            var b = false;
            TheRecord.SetNullableBoolean(9, b);
            Assert.AreEqual(TheRecord.GetBoolean(9), b);

            b = true;
            TheRecord.SetNullableBoolean("TheBool", b);
            Assert.AreEqual(TheRecord.GetBoolean("TheBool"), b);

            TheRecord.SetNullableBoolean(9, null);
            Assert.IsNull(TheRecord.GetNullableBoolean(9));

            TheRecord.SetNullableBoolean("TheBool", null);
            Assert.IsNull(TheRecord.GetNullableBoolean("TheBool"));
        }

        [TestMethod]
        public void FloatExtensions()
        {
            float b = 12.45f;
            TheRecord.SetNullableFloat(11, b);
            Assert.AreEqual(TheRecord.GetFloat(11), b);

            b = 11111;
            TheRecord.SetNullableFloat("TheFloat", b);
            Assert.AreEqual(TheRecord.GetFloat("TheFloat"), b);

            TheRecord.SetNullableFloat(11, null);
            Assert.IsNull(TheRecord.GetNullableFloat(11));

            TheRecord.SetNullableFloat("TheFloat", null);
            Assert.IsNull(TheRecord.GetNullableFloat("TheFloat"));
        }

        [TestMethod]
        public void DoubleExtensions()
        {
            double b = 87492743.232;
            TheRecord.SetNullableDouble(12, b);
            Assert.AreEqual(TheRecord.GetDouble(12), b);

            b = -23123.11;
            TheRecord.SetNullableDouble("TheDouble", b);
            Assert.AreEqual(TheRecord.GetDouble("TheDouble"), b);

            TheRecord.SetNullableDouble(12, null);
            Assert.IsNull(TheRecord.GetNullableDouble(12));

            TheRecord.SetNullableDouble("TheDouble", null);
            Assert.IsNull(TheRecord.GetNullableDouble("TheDouble"));
        }

        [TestMethod]
        public void GuidExtensions()
        {
            var b = new Guid(10, 2, 2, 5, 6, 4, 6, 7, 4, 3, 6);
            TheRecord.SetNullableGuid(13, b);
            Assert.AreEqual(TheRecord.GetGuid(13), b);

            b = new Guid(10, 2, 2, 115, 116, 114, 16, 7, 4, 3, 6);
            TheRecord.SetNullableGuid("TheGuid", b);
            Assert.AreEqual(TheRecord.GetGuid("TheGuid"), b);

            TheRecord.SetNullableGuid(13, null);
            Assert.IsNull(TheRecord.GetNullableGuid(13));

            TheRecord.SetNullableGuid("TheGuid", null);
            Assert.IsNull(TheRecord.GetNullableGuid("TheGuid"));
        }
    }
}
