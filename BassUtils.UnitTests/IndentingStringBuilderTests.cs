using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BassUtils.UnitTests
{
    [TestClass]
    public class IndentingStringBuilderTests
    {
        [TestMethod]
        public void GeneratesText()
        {
            var sb = new IndentingStringBuilder();
            sb.AppendLine("namespace MYNAMES");

            using (sb.BeginCodeBlock())
            {
                sb.AppendLine("public class Foo");
                using (sb.BeginCodeBlock())
                {
                    sb.AppendLines("public int XXX { get; set; }",
                                                 "public string VVVV { get; }");
                }
            }

            var result = sb.ToString();
        }
    }
}
