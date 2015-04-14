using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace BassUtils.Tests
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void SetChar_WhenValueIsNull_ThrowsArgumentNullException()
        {
            string s = null;
            var ex = Assert.Throws<ArgumentNullException>(() => s.SetChar(0, 'a'));
            Assert.AreEqual("value", ex.ParamName);
        }

        [Test]
        public void SetChar_WhenIndexIsLessThanZero_ThrowsArgumentOutOfRangeException()
        {
            string s = "hello";
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => s.SetChar(-1, 'a'));
            Assert.AreEqual("index", ex.ParamName);
        }

        [Test]
        public void SetChar_WhenIndexIsPastTheEndOfTheString_ThrowsArgumentOutOfRangeException()
        {
            string s = "hello";
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => s.SetChar(5, 'a'));
            Assert.AreEqual("index", ex.ParamName);
        }

        [Test]
        public void SetChar_WhenIndexIsOK_ReplacesCharacterCorrectly()
        {
            string s = "hello";
            s = s.SetChar(0, 'a');
            s = s.SetChar(4, 'z');
            Assert.AreEqual("aellz", s);
        }
    }
}
