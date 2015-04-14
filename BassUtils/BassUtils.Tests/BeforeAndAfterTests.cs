using System;
using NUnit.Framework;

namespace BassUtils.Tests
{
    [TestFixture]
    public class BeforeAndAfterTests
    {
        [Test]
        public void BeforeOnNullReturnsNull()
        {
            string s = null;
            string result = s.Before("foo");
            Assert.Null(result);
        }

        [Test]
        public void AfterOnNullReturnsNull()
        {
            string s = null;
            string result = s.After("foo");
            Assert.Null(result);
        }

        [Test]
        public void BeforeNullThrowsArgumentNullException()
        {
            string s = "foo";
            Assert.Throws<ArgumentNullException>(() => s.Before(null));
        }

        [Test]
        public void AfterNullThrowsArgumentNullException()
        {
            string s = "foo";
            Assert.Throws<ArgumentNullException>(() => s.After(null));
        }

        [Test]
        public void BeforeNotFoundStringReturnsNull()
        {
            string s = "foo";
            string result = s.Before("bar");
            Assert.Null(result);
        }

        [Test]
        public void AfterNotFoundStringReturnsNull()
        {
            string s = "foo";
            string result = s.After("bar");
            Assert.Null(result);
        }

        [Test]
        public void BeforeCaseSensitive()
        {
            string s = "fooFooFOO";
            string result = s.Before("Foo", StringComparison.InvariantCulture);
            Assert.AreEqual("foo", result);
        }

        [Test]
        public void AfterCaseSensitive()
        {
            string s = "fooFooFOO";
            string result = s.After("Foo", StringComparison.InvariantCulture);
            Assert.AreEqual("FOO", result);
        }

        [Test]
        public void BeforeCaseInSensitive()
        {
            string s = "fooFooFOO";
            string result = s.Before("Foo", StringComparison.InvariantCultureIgnoreCase);
            Assert.AreEqual("", result);
        }

        [Test]
        public void AfterCaseInSensitive()
        {
            string s = "fooFooFOO";
            string result = s.After("Foo", StringComparison.InvariantCultureIgnoreCase);
            Assert.AreEqual("FooFOO", result);
        }

        [Test]
        public void BeforeBeginningOfString()
        {
            string s = "abc";
            string result = s.Before("a");
            Assert.AreEqual("", result);
        }

        [Test]
        public void AfterBeginningOfString()
        {
            string s = "abc";
            string result = s.After("a");
            Assert.AreEqual("bc", result);
        }

        [Test]
        public void BeforeEndOfString()
        {
            string s = "abc";
            string result = s.Before("c");
            Assert.AreEqual("ab", result);
        }

        [Test]
        public void AfterEndOfString()
        {
            string s = "abc";
            string result = s.After("c");
            Assert.AreEqual("", result);
        }

        [Test]
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
