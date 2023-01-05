using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using USFMToolsSharp.Linter.Models;
using USFMToolsSharp.Models.Markers;

namespace USFMToolsSharp.Linter.Test
{

    [TestClass]
    public class USFMLinterTest
    {

        [TestMethod]
        public void TestBasic()
        {
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestParse()
        {
            USFMParser parser = new USFMParser();
            USFMDocument doc = parser.ParseFromString("\\v 1 In the beginning \\bd God \\bd*");
            USFMLinter linter = new USFMLinter();
            List<LinterResult> results = linter.Lint(doc);
            Assert.AreEqual(0, results.Count);
        }



    }
}