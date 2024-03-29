﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BassUtils.UnitTests
{
    [TestClass]
    public class StringExtensionsTests_PadAndAlignTests
    {
        [TestMethod]
        public void MinWidthCannotBeNegative()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => "".PadAndAlign(-1, 3, PaddingAlignment.Left, ' '));
        }

        [TestMethod]
        public void MaxWidthCannotBeNegative()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => "".PadAndAlign(2, -1, PaddingAlignment.Left, ' '));
        }

        [TestMethod]
        public void MinWidthCanBeZero()
        {
            "".PadAndAlign(0, 3, PaddingAlignment.Left, ' ');
        }

        [TestMethod]
        public void MaxWidthCanBeZero()
        {
            "".PadAndAlign(0, 0, PaddingAlignment.Left, ' ');
        }

        [TestMethod]
        public void MinWidthMustBeLessThanMaxWidth()
        {
            string text = "";
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => text.PadAndAlign(5, 3, PaddingAlignment.Left, ' '));
        }

        [TestMethod]
        public void NullAndEmptyInputStringsAreTreatedTheSame()
        {
            string text1 = null, text2 = "";
            string result1 = text1.PadAndAlign(3, 3, PaddingAlignment.Left, '*');
            string result2 = text2.PadAndAlign(3, 3, PaddingAlignment.Left, '*');
            Assert.AreEqual("***", result1);
            Assert.AreEqual("***", result2);
        }

        [TestMethod]
        public void AlignLeftWorks()
        {
            AlignLeftTest(null, "***");
            AlignLeftTest("", "***");
            AlignLeftTest("a", "a**");
            AlignLeftTest("ab", "ab*");
            AlignLeftTest("abc", "abc");
            AlignLeftTest("abcd", "abcd");
            AlignLeftTest("abcde", "abcde");
            AlignLeftTest("abcdef", "abcde");
        }

        void AlignLeftTest(string text, string expected)
        {
            string result = text.PadAndAlign(3, 5, PaddingAlignment.Left, '*');
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void AlignRightWorks()
        {
            AlignRightTest(null, "***");
            AlignRightTest("", "***");
            AlignRightTest("a", "**a");
            AlignRightTest("ab", "*ab");
            AlignRightTest("abc", "abc");
            AlignRightTest("abcd", "abcd");
            AlignRightTest("abcde", "abcde");
            AlignRightTest("abcdef", "bcdef");
        }

        void AlignRightTest(string text, string expected)
        {
            string result = text.PadAndAlign(3, 5, PaddingAlignment.Right, '*');
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void AlignCenterWorks()
        {
            AlignCenterTest(null, "***");
            AlignCenterTest("", "***");
            AlignCenterTest("a", "*a*");
            AlignCenterTest("ab", "ab*");
            AlignCenterTest("abc", "abc");
            AlignCenterTest("abcd", "abcd");
            AlignCenterTest("abcde", "abcde");
            AlignCenterTest("abcdef", "abcde");
            // one more case here compared to left and right.
            AlignCenterTest("abcdefg", "bcdef");
        }

        void AlignCenterTest(string text, string expected)
        {
            string result = text.PadAndAlign(3, 5, PaddingAlignment.Center, '*');
            Assert.AreEqual(expected, result);
        }
    }
}
