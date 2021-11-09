﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BassUtils.UnitTests
{
    [TestClass]
    public class StringExtensionTests_BeforeAndAfter
    {
        [TestMethod]
        public void BeforeOnNullReturnsNull()
        {
            string s = null;
            string result = s.Before("foo");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AfterOnNullReturnsNull()
        {
            string s = null;
            string result = s.After("foo");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void BeforeNullThrowsArgumentNullException()
        {
            string s = "foo";
            Assert.ThrowsException<ArgumentNullException>(() => s.Before(null));
        }

        [TestMethod]
        public void AfterNullThrowsArgumentNullException()
        {
            string s = "foo";
            Assert.ThrowsException<ArgumentNullException>(() => s.After(null));
        }

        [TestMethod]
        public void BeforeNotFoundStringReturnsNull()
        {
            string s = "foo";
            string result = s.Before("bar");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AfterNotFoundStringReturnsNull()
        {
            string s = "foo";
            string result = s.After("bar");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void BeforeCaseSensitive()
        {
            string s = "fooFooFOO";
            string result = s.Before("Foo", StringComparison.InvariantCulture);
            Assert.AreEqual("foo", result);
        }

        [TestMethod]
        public void AfterCaseSensitive()
        {
            string s = "fooFooFOO";
            string result = s.After("Foo", StringComparison.InvariantCulture);
            Assert.AreEqual("FOO", result);
        }

        [TestMethod]
        public void BeforeCaseInSensitive()
        {
            string s = "fooFooFOO";
            string result = s.Before("Foo", StringComparison.InvariantCultureIgnoreCase);
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void AfterCaseInSensitive()
        {
            string s = "fooFooFOO";
            string result = s.After("Foo", StringComparison.InvariantCultureIgnoreCase);
            Assert.AreEqual("FooFOO", result);
        }

        [TestMethod]
        public void BeforeBeginningOfString()
        {
            string s = "abc";
            string result = s.Before("a");
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void AfterBeginningOfString()
        {
            string s = "abc";
            string result = s.After("a");
            Assert.AreEqual("bc", result);
        }

        [TestMethod]
        public void BeforeEndOfString()
        {
            string s = "abc";
            string result = s.Before("c");
            Assert.AreEqual("ab", result);
        }

        [TestMethod]
        public void AfterEndOfString()
        {
            string s = "abc";
            string result = s.After("c");
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void BeforeAndAfter()
        {
            string s = "abc;;AZ;;def";
            string before, after;
            s.BeforeAndAfter(";;AZ;;", StringComparison.InvariantCultureIgnoreCase, out before, out after);
            Assert.AreEqual("abc", before);
            Assert.AreEqual("def", after);
        }
    }
}
