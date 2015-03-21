using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace BassUtils.Tests
{
    [TestFixture]
    public class ConvTests
    {
        [Test]
        [TestCaseSource("StringToBestCases")]
        public void StringToBest_ForVarious_ReturnsExpected(string value, object expected)
        {
            object result = Conv.StringToBest(value, CultureInfo.InvariantCulture);
            Assert.AreEqual(expected, result);
            // The very first test case passes two nulls, which we have to be careful of.
            if (expected != null && result != null)
                Assert.AreEqual(expected.GetType(), result.GetType());
        }

        static object[] StringToBestCases = 
        {
            new object[] { null, (string)null },
            new object[] { "", "" },
            new object[] { Data.Whitespace, Data.Whitespace },
            new object[] { "hello", "hello" },
            new object[] { "2014-02-04 11:34:12", new DateTime(2014, 2, 4, 11, 34, 12) },
            new object[] { "2014-02-04", new DateTime(2014, 2, 4) },
            new object[] { "true", true },
            new object[] { "True", true },
            new object[] { "false", false },
            new object[] { "False", false },

            new object[] { "FEDCBA98-1234-1234-1234-FEDCBA987654", new Guid("FEDCBA98-1234-1234-1234-FEDCBA987654") },
            new object[] { "{FEDCBA98-1234-1234-1234-FEDCBA987654}", new Guid("{FEDCBA98-1234-1234-1234-FEDCBA987654}") },
            new object[] { "(FEDCBA98-1234-1234-1234-FEDCBA987654)", new Guid("(FEDCBA98-1234-1234-1234-FEDCBA987654)") },
            new object[] { "{0xFEDCBA98,0x1234,0x1234,{0x12,0x34,0xFE,0xDC,0xBA,0x98,0x76,0x54}}", new Guid("{0xFEDCBA98,0x1234,0x1234,{0x12,0x34,0xFE,0xDC,0xBA,0x98,0x76,0x54}}") },

            new object[] { "12:34:56", new TimeSpan(12, 34, 56) },
            new object[] { "12:34:56.789", new TimeSpan(0, 12, 34, 56, 789) },

            new object[] { "7.12:34:56", new TimeSpan(7, 12, 34, 56) },
            new object[] { "7.12:34:56.789", new TimeSpan(7, 12, 34, 56, 789) },

            new object[] { "0xFF", 0xFF },                          // You can have uint hex literals in C#, but I don't care, yet.
            new object[] { "0xAABBCCDDEEFF", 0xAABBCCDDEEFFL },

            new object[] { "0", 0 },
            new object[] { "0.0", 0D },
            new object[] { "0m", 0m },

            new object[] { "789", 789 },
            new object[] { "789m", 789m },
            new object[] { "789D", 789D },
            new object[] { "789.0", 789D },
            new object[] { "789.123", 789.123D },
            new object[] { "789.123m", 789.123m },
            new object[] { "789e5", 789e5D },                       // Only doubles can have exponents.
            new object[] { "789e5D", 789e5D },
            new object[] { "4294967296", 4294967296L },
            new object[] { "4294967296L", 4294967296L },

            new object[] { "456,789", 456789 },
            new object[] { "456,789m", 456789m },
            new object[] { "456,789D", 456789D },
            new object[] { "456,789.0", 456789D },
            new object[] { "456,789.123", 456789.123D },
            new object[] { "456,789e5", 456789e5D },                // Only doubles can have exponents.
            new object[] { "456,789e5D", 456789e5D },
            new object[] { "456,789.123m", 456789.123m },
            new object[] { "4,294,967,296", 4294967296L },
            new object[] { "4,294,967,296L", 4294967296L },

            new object[] { "-789", -789 },
            new object[] { "-789m", -789m },
            new object[] { "-789D", -789D },
            new object[] { "-789.0", -789D },
            new object[] { "-789.123", -789.123D },
            new object[] { "-789.123m", -789.123m },
            new object[] { "-789e5", -789e5D },                     // Only doubles can have exponents.
            new object[] { "-789e5D", -789e5D },
            new object[] { "-4294967296", -4294967296L },
            new object[] { "-4294967296L", -4294967296L },

            new object[] { "-456,789", -456789 },
            new object[] { "-456,789m", -456789m },
            new object[] { "-456,789D", -456789D },
            new object[] { "-456,789.0", -456789D },
            new object[] { "-456,789.123", -456789.123D },
            new object[] { "-456,789e5", -456789e5D },              // Only doubles can have exponents.
            new object[] { "-456,789e5D", -456789e5D },
            new object[] { "-456,789.123m", -456789.123m },
            new object[] { "-4,294,967,296", -4294967296L },
            new object[] { "-4,294,967,296L", -4294967296L }
        };
    }
}
