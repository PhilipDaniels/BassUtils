using System;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace BassUtils.Tests
{
    [TestFixture]
    public class ArgumentValidatorTests
    {
        [Test]
        public void ThrowIfNull_WhenGivenNullAndNullMessage_ThrowsArgumentNullException()
        {
            string s = null;
            var ex = Assert.Throws<ArgumentNullException>(() => s.ThrowIfNull("s"));
            Assert.AreEqual(ex.ParamName, "s");
        }

        [Test]
        public void ThrowIfNull_WhenGivenNullAndExplicitMessage_ThrowsArgumentNullException()
        {
            string s = null;
            var ex = Assert.Throws<ArgumentNullException>(() => s.ThrowIfNull("s", "my msg"));
            Assert.AreEqual("s", ex.ParamName);
            Assert.That(ex.Message, Is.StringStarting("my msg"));
        }

        [Test]
        public void ThrowIfNull_WhenGivenNonNull_ReturnsTheSameObject()
        {
            string s = "";
            string p = s.ThrowIfNull("s");
            Assert.AreSame(s, p);
        }

        [Test]
        public void ThrowIfNullOrEmpty_WhenGivenNullAndNullMessage_ThrowsArgumentNullException()
        {
            string s = null;
            var ex = Assert.Throws<ArgumentNullException>(() => s.ThrowIfNullOrEmpty("s"));
            Assert.AreEqual(ex.ParamName, "s");
        }

        [Test]
        public void ThrowIfNullOrEmpty_WhenGivenNullAndExplicitMessage_ThrowsArgumentNullException()
        {
            string s = null;
            var ex = Assert.Throws<ArgumentNullException>(() => s.ThrowIfNullOrEmpty("s", "my msg"));
            Assert.AreEqual("s", ex.ParamName);
            Assert.That(ex.Message, Is.StringStarting("my msg"));
        }

        [Test]
        public void ThrowIfNullOrEmpty_WhenGivenEmptyAndNullMessage_ThrowsArgumentException()
        {
            string s = "";
            var ex = Assert.Throws<ArgumentException>(() => s.ThrowIfNullOrEmpty("s"));
            Assert.AreEqual(ex.ParamName, "s");
        }

        [Test]
        public void ThrowIfNullOrEmpty_WhenGivenEmptyAndExplicitMessage_ThrowsArgumentException()
        {
            string s = "";
            var ex = Assert.Throws<ArgumentException>(() => s.ThrowIfNullOrEmpty("s", "my msg"));
            Assert.AreEqual("s", ex.ParamName);
            Assert.That(ex.Message, Is.StringStarting("my msg"));
        }

        [Test]
        public void ThrowIfNullOrEmpty_WhenGivenNonEmpty_ReturnsTheSameObject()
        {
            string s = " ";
            string p = s.ThrowIfNullOrEmpty("s");
            Assert.AreSame(s, p);
        }

        [Test]
        public void ThrowIfNullOrWhiteSpace_WhenGivenNullAndNullMessage_ThrowsArgumentNullException()
        {
            string s = null;
            var ex = Assert.Throws<ArgumentNullException>(() => s.ThrowIfNullOrWhiteSpace("s"));
            Assert.AreEqual(ex.ParamName, "s");
        }

        [Test]
        public void ThrowIfNullOrWhiteSpace_WhenGivenNullAndExplicitMessage_ThrowsArgumentNullException()
        {
            string s = null;
            var ex = Assert.Throws<ArgumentNullException>(() => s.ThrowIfNullOrWhiteSpace("s", "my msg"));
            Assert.AreEqual("s", ex.ParamName);
            Assert.That(ex.Message, Is.StringStarting("my msg"));
        }

        [Test]
        public void ThrowIfNullOrWhiteSpace_WhenGivenEmptyAndNullMessage_ThrowsArgumentException()
        {
            string s = "";
            var ex = Assert.Throws<ArgumentException>(() => s.ThrowIfNullOrWhiteSpace("s"));
            Assert.AreEqual(ex.ParamName, "s");
        }

        [Test]
        public void ThrowIfNullOrWhiteSpace_WhenGivenEmptyAndExplicitMessage_ThrowsArgumentException()
        {
            string s = "";
            var ex = Assert.Throws<ArgumentException>(() => s.ThrowIfNullOrWhiteSpace("s", "my msg"));
            Assert.AreEqual("s", ex.ParamName);
            Assert.That(ex.Message, Is.StringStarting("my msg"));
        }

        [Test]
        public void ThrowIfNullOrWhiteSpace_WhenGivenWhitespaceAndNullMessage_ThrowsArgumentException()
        {
            string s = Data.Whitespace;
            var ex = Assert.Throws<ArgumentException>(() => s.ThrowIfNullOrWhiteSpace("s"));
            Assert.AreEqual(ex.ParamName, "s");
        }

        [Test]
        public void ThrowIfNullOrWhiteSpace_WhenGivenWhitespaceAndExplicitMessage_ThrowsArgumentException()
        {
            string s = Data.Whitespace;
            var ex = Assert.Throws<ArgumentException>(() => s.ThrowIfNullOrWhiteSpace("s", "my msg"));
            Assert.AreEqual("s", ex.ParamName);
            Assert.That(ex.Message, Is.StringStarting("my msg"));
        }

        [Test]
        public void ThrowIfNullOrWhiteSpace_WhenGivenNonWhitespace_ReturnsTheSameObject()
        {
            string s = "ok";
            string p = s.ThrowIfNullOrWhiteSpace("s");
            Assert.AreSame(s, p);
        }

        [Test]
        public void ThrowIfLessThan_WhenGivenNullLeftHandSide_ThrowsArgumentNullException()
        {
            string s = null;
            string t = "T";
            var ex = Assert.Throws<ArgumentNullException>(() => s.ThrowIfLessThan(t, "t"));
            Assert.AreEqual("t", ex.ParamName);
        }

        [Test]
        public void ThrowIfLessThan_WhenCheckFailsWithNullMesssage_ThrowsArgumentOutOfRangeException()
        {
            int val = 0;
            int bound = 1;
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => val.ThrowIfLessThan(bound, "val"));
            Assert.AreEqual("val", ex.ParamName);
            Assert.AreEqual(val, ex.ActualValue);
            Assert.That(ex.Message, Is.StringContaining("val"));
            Assert.That(ex.Message, Is.StringContaining(val.ToString()));
            Assert.That(ex.Message, Is.StringContaining(bound.ToString()));
        }

        [Test]
        public void ThrowIfLessThan_WhenCheckFailsWithExplicitMesssage_ThrowsArgumentOutOfRangeException()
        {
            int val = 0;
            int bound = 1;
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => val.ThrowIfLessThan(bound, "val", "my msg"));
            Assert.AreEqual("val", ex.ParamName);
            Assert.AreEqual(val, ex.ActualValue);
            Assert.That(ex.Message, Is.StringContaining("val"));
            Assert.That(ex.Message, Is.StringContaining(val.ToString()));
            Assert.That(ex.Message, Is.StringContaining(bound.ToString()));
            Assert.That(ex.Message, Is.StringStarting("my msg"));
        }

        [Test]
        public void ThrowIfLessThan_WhenCheckPasses_ReturnsSameObject()
        {
            string s1 = "ok";
            string s2 = "ok";
            string result = s1.ThrowIfLessThan(s2, "s2");
            Assert.AreSame(s1, result);
        }

        [Test]
        public void ThrowIfLessThanOrEqualTo_WhenGivenNullLeftHandSide_ThrowsArgumentNullException()
        {
            string s = null;
            string t = "T";
            var ex = Assert.Throws<ArgumentNullException>(() => s.ThrowIfLessThanOrEqualTo(t, "t"));
            Assert.AreEqual("t", ex.ParamName);
        }

        [Test]
        public void ThrowIfLessThanOrEqualTo_WhenCheckFailsWithNullMesssage_ThrowsArgumentOutOfRangeException()
        {
            int val = 0;
            int bound = 0;
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => val.ThrowIfLessThanOrEqualTo(bound, "val"));
            Assert.AreEqual("val", ex.ParamName);
            Assert.AreEqual(val, ex.ActualValue);
            Assert.That(ex.Message, Is.StringContaining("val"));
            Assert.That(ex.Message, Is.StringContaining(val.ToString()));
            Assert.That(ex.Message, Is.StringContaining(bound.ToString()));
        }

        [Test]
        public void ThrowIfLessThanOrEqualTo_WhenCheckFailsWithExplicitMesssage_ThrowsArgumentOutOfRangeException()
        {
            int val = 0;
            int bound = 0;
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => val.ThrowIfLessThanOrEqualTo(bound, "val", "my msg"));
            Assert.AreEqual("val", ex.ParamName);
            Assert.AreEqual(val, ex.ActualValue);
            Assert.That(ex.Message, Is.StringContaining("val"));
            Assert.That(ex.Message, Is.StringContaining(val.ToString()));
            Assert.That(ex.Message, Is.StringContaining(bound.ToString()));
            Assert.That(ex.Message, Is.StringStarting("my msg"));
        }

        [Test]
        public void ThrowIfLessThanOrEqualTo_WhenCheckPasses_ReturnsSameObject()
        {
            string s1 = "zok";
            string s2 = "ok";
            string result = s1.ThrowIfLessThanOrEqualTo(s2, "s2");
            Assert.AreSame(s1, result);
        }
        
        //ThrowIfMoreThan
        //ThrowIfMoreThanOrEqualTo

        [Test]
        public void ThrowIfMoreThan_WhenGivenNullLeftHandSide_ThrowsArgumentNullException()
        {
            string s = null;
            string t = "T";
            var ex = Assert.Throws<ArgumentNullException>(() => s.ThrowIfMoreThan(t, "t"));
            Assert.AreEqual("t", ex.ParamName);
        }

        [Test]
        public void ThrowIfMoreThan_WhenCheckFailsWithNullMesssage_ThrowsArgumentOutOfRangeException()
        {
            int val = 1;
            int bound = 0;
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => val.ThrowIfMoreThan(bound, "val"));
            Assert.AreEqual("val", ex.ParamName);
            Assert.AreEqual(val, ex.ActualValue);
            Assert.That(ex.Message, Is.StringContaining("val"));
            Assert.That(ex.Message, Is.StringContaining(val.ToString()));
            Assert.That(ex.Message, Is.StringContaining(bound.ToString()));
        }

        [Test]
        public void ThrowIfMoreThan_WhenCheckFailsWithExplicitMesssage_ThrowsArgumentOutOfRangeException()
        {
            int val = 1;
            int bound = 0;
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => val.ThrowIfMoreThan(bound, "val", "my msg"));
            Assert.AreEqual("val", ex.ParamName);
            Assert.AreEqual(val, ex.ActualValue);
            Assert.That(ex.Message, Is.StringContaining("val"));
            Assert.That(ex.Message, Is.StringContaining(val.ToString()));
            Assert.That(ex.Message, Is.StringContaining(bound.ToString()));
            Assert.That(ex.Message, Is.StringStarting("my msg"));
        }

        [Test]
        public void ThrowIfMoreThan_WhenCheckPasses_ReturnsSameObject()
        {
            string s1 = "ok";
            string s2 = "ok";
            string result = s1.ThrowIfMoreThan(s2, "s2");
            Assert.AreSame(s1, result);
        }

        [Test]
        public void ThrowIfMoreThanOrEqualTo_WhenGivenNullLeftHandSide_ThrowsArgumentNullException()
        {
            string s = null;
            string t = "T";
            var ex = Assert.Throws<ArgumentNullException>(() => s.ThrowIfMoreThanOrEqualTo(t, "t"));
            Assert.AreEqual("t", ex.ParamName);
        }

        [Test]
        public void ThrowIfMoreThanOrEqualTo_WhenCheckFailsWithNullMesssage_ThrowsArgumentOutOfRangeException()
        {
            int val = 0;
            int bound = 0;
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => val.ThrowIfMoreThanOrEqualTo(bound, "val"));
            Assert.AreEqual("val", ex.ParamName);
            Assert.AreEqual(val, ex.ActualValue);
            Assert.That(ex.Message, Is.StringContaining("val"));
            Assert.That(ex.Message, Is.StringContaining(val.ToString()));
            Assert.That(ex.Message, Is.StringContaining(bound.ToString()));
        }

        [Test]
        public void ThrowIfMoreThanOrEqualTo_WhenCheckFailsWithExplicitMesssage_ThrowsArgumentOutOfRangeException()
        {
            int val = 0;
            int bound = 0;
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => val.ThrowIfMoreThanOrEqualTo(bound, "val", "my msg"));
            Assert.AreEqual("val", ex.ParamName);
            Assert.AreEqual(val, ex.ActualValue);
            Assert.That(ex.Message, Is.StringContaining("val"));
            Assert.That(ex.Message, Is.StringContaining(val.ToString()));
            Assert.That(ex.Message, Is.StringContaining(bound.ToString()));
            Assert.That(ex.Message, Is.StringStarting("my msg"));
        }

        [Test]
        public void ThrowIfMoreThanOrEqualTo_WhenCheckPasses_ReturnsSameObject()
        {
            string s1 = "ok";
            string s2 = "zok";
            string result = s1.ThrowIfMoreThanOrEqualTo(s2, "s2");
            Assert.AreSame(s1, result);
        }

        [Test]
        public void ThrowIfFileDoesNotExist_WhenPassedNull_ThrowsArgumentNullException()
        {
            string f = null;
            var ex = Assert.Throws<ArgumentNullException>(() => f.ThrowIfFileDoesNotExist("f"));
            Assert.AreEqual("f", ex.ParamName);
        }

        [Test]
        public void ThrowIfFileDoesNotExist_WhenPassedEmptyString_ThrowsArgumentException()
        {
            string f = "";
            var ex = Assert.Throws<ArgumentException>(() => f.ThrowIfFileDoesNotExist("f"));
            Assert.AreEqual("f", ex.ParamName);
        }

        [Test]
        public void ThrowIfFileDoesNotExist_WhenPassedWhitespaceString_ThrowsArgumentException()
        {
            string f = Data.Whitespace;
            var ex = Assert.Throws<ArgumentException>(() => f.ThrowIfFileDoesNotExist("f"));
            Assert.AreEqual("f", ex.ParamName);
        }

        [Test]
        public void ThrowIfFileDoesNotExist_WhenFileDoesNotExist_ThrowsFileNotFoundException()
        {
            string f = @"z:\hahahah\" + Guid.NewGuid().ToString();
            var ex = Assert.Throws<FileNotFoundException>(() => f.ThrowIfFileDoesNotExist("fname"));
            Assert.AreEqual(f, ex.FileName);
            Assert.That(ex.Message, Is.StringContaining(f));
            Assert.That(ex.Message, Is.StringContaining("fname"));
        }

        [Test]
        public void ThrowIfFileDoesNotExist_WhenFileExists_ReturnsPath()
        {
            string f = null;

            try
            { 
                f = Path.GetTempFileName();
                string path = f.ThrowIfFileDoesNotExist("fname");
                Assert.AreEqual(f, path);
            }
            catch
            {
                Assert.Fail("No exception should be thrown.");
            }
            finally
            {
                if (f != null)
                    File.Delete(f);
            }
        }

        [Test]
        public void ThrowIfDirectoryDoesNotExist_WhenPassedNull_ThrowsArgumentNullException()
        {
            string dir = null;
            var ex = Assert.Throws<ArgumentNullException>(() => dir.ThrowIfDirectoryDoesNotExist("dirname"));
            Assert.AreEqual("dirname", ex.ParamName);
        }

        [Test]
        public void ThrowIfDirectoryDoesNotExist_WhenPassedEmptyString_ThrowsArgumentException()
        {
            string dir = "";
            var ex = Assert.Throws<ArgumentException>(() => dir.ThrowIfDirectoryDoesNotExist("dirname"));
            Assert.AreEqual("dirname", ex.ParamName);
        }

        [Test]
        public void ThrowIfDirectoryDoesNotExist_WhenPassedWhitespaceString_ThrowsArgumentException()
        {
            string dir = Data.Whitespace;
            var ex = Assert.Throws<ArgumentException>(() => dir.ThrowIfDirectoryDoesNotExist("dirname"));
            Assert.AreEqual("dirname", ex.ParamName);
        }

        [Test]
        public void ThrowIfDirectoryDoesNotExist_WhenFileDoesNotExist_ThrowsDirectoryNotFoundException()
        {
            string dir = @"z:\hahahah\" + Guid.NewGuid().ToString();
            var ex = Assert.Throws<DirectoryNotFoundException>(() => dir.ThrowIfDirectoryDoesNotExist("dirname"));
            Assert.That(ex.Message, Is.StringContaining(dir));
            Assert.That(ex.Message, Is.StringContaining("dirname"));
        }

        [Test]
        public void ThrowIfDirectoryDoesNotExist_WhenDirectoryExists_ReturnsPath()
        {
            string f = null;
            string dir = null;

            try
            {
                f = Path.GetTempFileName();
                dir = Path.GetDirectoryName(f);
                string path = dir.ThrowIfDirectoryDoesNotExist("dirname");
                Assert.AreEqual(dir, path);
            }
            catch
            {
                Assert.Fail("No exception should be thrown.");
            }
            finally
            {
                if (f != null)
                    File.Delete(f);
            }
        }

        [Test]
        public void ThrowIfInvalidEnumerand_WhenCalledOnNonEnumeratedType_ThrowsArgumentException()
        {
            int x = 42;
            var ex = Assert.Throws<ArgumentException>(() => x.ThrowIfInvalidEnumerand("x"));
            Assert.That(ex.Message, Is.StringContaining("System.Int32"));
        }

        enum Cars { Ford = 1, Volvo = 2};

        [Test]
        public void ThrowIfInvalidEnumerand_WhenCalledWithOutOfRangeEnumerand_ThrowsArgumentException()
        {
            Cars car = 0;
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => car.ThrowIfInvalidEnumerand("car"));

            Assert.AreEqual("car", ex.ParamName);
            Assert.AreEqual(car, ex.ActualValue);
            Assert.That(ex.Message, Is.StringContaining("car"));
            Assert.That(ex.Message, Is.StringContaining(car.ToString()));
            Assert.That(ex.Message, Is.StringContaining(car.GetType().FullName));
        }
    }
}
