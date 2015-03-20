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
        //[TestCase("0", 0)]
        //[TestCase("0.0", 0D)]
        //[TestCase("0m", 0m)]

        //[TestCase("42", 42)]
        //[TestCase("42.0", 42D)]
        //[TestCase("134,567", 134567)]
        //[TestCase("42m", 42m)]
        //[TestCase("4294967296", 4294967296L)]
        //[TestCase("4294967296L", 4294967296L)]
        
        // hex => int

        //[TestCase("-42", -42)]
        //[TestCase("-42.0", -42D)]
        //[TestCase("-42m", -42m)]
        //[TestCase("42.12345", 42.12345D)]
        //[TestCase("42.12345m", 42.12345m)]
        //[TestCase("42e17", 42e17)]
        //[TestCase("42e17m", 42e17m)]
        //[TestCase("-42e17", -42e17)]
        //[TestCase("-42e17m", -42e17m)]
        //[TestCase("-4294967296", -4294967296L)]
        //[TestCase("-4294967296L", -4294967296L)]

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
            //new object[] { null, (string)null },
            //new object[] {"", ""},
            //new object[] {Data.Whitespace, Data.Whitespace},
            //new object[] { "2014-02-04 11:34:12", new DateTime(2014, 2, 4, 11, 34, 12) },
            //new object[] { "2014-02-04", new DateTime(2014, 2, 4) },
            //new object[] { "true", true },
            //new object[] { "True", true },
            //new object[] { "false", false },
            //new object[] { "False", false },

            new object[] { "0", 0 },
            new object[] { "0.0", 0D },
            new object[] { "0m", 0m }
        };
    }
}
