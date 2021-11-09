using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BassUtils.UnitTests
{
    [TestClass]
    public class ConvTests
    {
        [TestMethod]
        public void StringToBest_ForVarious_ReturnsExpected()
        {
            foreach (var testCase in StringToBestCases)
            {
                var inputString = testCase.Item1;
                object expected = testCase.Item2;

                object result = Conv.StringToBest(inputString, CultureInfo.InvariantCulture);
                Assert.AreEqual(expected, result);

                // The very first test case passes two nulls, which we have to be careful of.
                if (expected != null && result != null)
                    Assert.AreEqual(expected.GetType(), result.GetType());
            }
        }

        static readonly string Whitespace = " \t\n";

        static List<Tuple<string, object>> StringToBestCases = new List<Tuple<string, object>>()
        {
            new Tuple<string, object>(null, (string)null),
            new Tuple<string, object>("",""),
            new Tuple<string, object>(Whitespace,Whitespace),
            new Tuple<string, object>("hello", "hello"),
            new Tuple<string, object>("2014-02-04 11:34:12", new DateTime(2014, 2, 4, 11, 34, 12)),
            new Tuple<string, object>("2014-02-04", new DateTime(2014, 2, 4)),
            new Tuple<string, object>("true", true),
            new Tuple<string, object>("True", true),
            new Tuple<string, object>("false", false),
            new Tuple<string, object>("False", false),

            new Tuple<string, object>("FEDCBA98-1234-1234-1234-FEDCBA987654", new Guid("FEDCBA98-1234-1234-1234-FEDCBA987654")),
            new Tuple<string, object>("{FEDCBA98-1234-1234-1234-FEDCBA987654}", new Guid("{FEDCBA98-1234-1234-1234-FEDCBA987654}")),
            new Tuple<string, object>("(FEDCBA98-1234-1234-1234-FEDCBA987654)", new Guid("(FEDCBA98-1234-1234-1234-FEDCBA987654)")),
            new Tuple<string, object>("{0xFEDCBA98,0x1234,0x1234,{0x12,0x34,0xFE,0xDC,0xBA,0x98,0x76,0x54}}", new Guid("{0xFEDCBA98,0x1234,0x1234,{0x12,0x34,0xFE,0xDC,0xBA,0x98,0x76,0x54}}")),

            new Tuple<string, object>("12:34:56", new TimeSpan(12, 34, 56)),
            new Tuple<string, object>("12:34:56.789", new TimeSpan(0, 12, 34, 56, 789)),

            new Tuple<string, object>("7.12:34:56", new TimeSpan(7, 12, 34, 56)),
            new Tuple<string, object>("7.12:34:56.789", new TimeSpan(7, 12, 34, 56, 789)),

            new Tuple<string, object>("0xFF", 0xFF),                          // You can have uint hex literals in C#, but I don't care, yet.
            new Tuple<string, object>("0xAABBCCDDEEFF", 0xAABBCCDDEEFFL),

            new Tuple<string, object>("0", 0),
            new Tuple<string, object>("0.0", 0D),
            new Tuple<string, object>("0m", 0m),

            new Tuple<string, object>("789", 789),
            new Tuple<string, object>("789m", 789m),
            new Tuple<string, object>("789D", 789D),
            new Tuple<string, object>("789.0", 789D),
            new Tuple<string, object>("789.123", 789.123D),
            new Tuple<string, object>("789.123m", 789.123m),
            new Tuple<string, object>("789e5", 789e5D),                       // Only doubles can have exponents.
            new Tuple<string, object>("789e5D", 789e5D),
            new Tuple<string, object>("4294967296", 4294967296L),
            new Tuple<string, object>("4294967296L", 4294967296L),

            new Tuple<string, object>("456,789", 456789),
            new Tuple<string, object>("456,789m", 456789m),
            new Tuple<string, object>("456,789D", 456789D),
            new Tuple<string, object>("456,789.0", 456789D),
            new Tuple<string, object>("456,789.123", 456789.123D),
            new Tuple<string, object>("456,789e5", 456789e5D),                // Only doubles can have exponents.
            new Tuple<string, object>("456,789e5D", 456789e5D),
            new Tuple<string, object>("456,789.123m", 456789.123m),
            new Tuple<string, object>("4,294,967,296", 4294967296L),
            new Tuple<string, object>("4,294,967,296L", 4294967296L),

            new Tuple<string, object>("-789", -789),
            new Tuple<string, object>("-789m", -789m),
            new Tuple<string, object>("-789D", -789D),
            new Tuple<string, object>("-789.0", -789D),
            new Tuple<string, object>("-789.123", -789.123D),
            new Tuple<string, object>("-789.123m", -789.123m),
            new Tuple<string, object>("-789e5", -789e5D),                     // Only doubles can have exponents.
            new Tuple<string, object>("-789e5D", -789e5D),
            new Tuple<string, object>("-4294967296", -4294967296L),
            new Tuple<string, object>("-4294967296L", -4294967296L),

            new Tuple<string, object>("-456,789", -456789),
            new Tuple<string, object>("-456,789m", -456789m),
            new Tuple<string, object>("-456,789D", -456789D),
            new Tuple<string, object>("-456,789.0", -456789D),
            new Tuple<string, object>("-456,789.123", -456789.123D),
            new Tuple<string, object>("-456,789e5", -456789e5D),              // Only doubles can have exponents.
            new Tuple<string, object>("-456,789e5D", -456789e5D),
            new Tuple<string, object>("-456,789.123m", -456789.123m),
            new Tuple<string, object>("-4,294,967,296", -4294967296L),
            new Tuple<string, object>("-4,294,967,296L", -4294967296L)
        };
    }
}
