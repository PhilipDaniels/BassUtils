using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace BassUtils.Tests
{
    [TestFixture]
    public class StreamExtensionTests
    {
        [Test]
        public void ReadFully_WhenPassedNullStream_ThrowsArgumentNullException()
        {
            Stream s = null;
            var ex = Assert.Throws<ArgumentNullException>(() => s.ReadFully());
            Assert.AreEqual(ex.ParamName, "stream");
        }

        [Test]
        public void ReadFully_WhenPassedStreamLargerThanBufferSize_ReadsWholeStream()
        {
            byte[] input = new byte[40000];
            for (int i = 0; i < input.Length; i++)
                input[i] = (byte)(i % 256);

            using (var sourceStream = new MemoryStream(input))
            {
                byte[] output = sourceStream.ReadFully();
                Assert.AreEqual(input, output);
            }
        }
    }
}
