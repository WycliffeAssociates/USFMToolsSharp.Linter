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
        public void TestNoWarnings()
        {
            USFMParser parser = new();
            USFMDocument doc = parser.ParseFromString("\\v 1 In the beginning \\bd God \\bd*");
            USFMLinter linter = new USFMLinter();
            List<LinterResult> results = linter.Lint(doc);
            Assert.AreEqual(0, results.Count);
        }


        [TestMethod]
        public void TestUnknownMarker()
        {
            USFMParser parser = new();
            USFMDocument doc = parser.ParseFromString("Invalid marker: \\cheese");
            USFMLinter linter = new USFMLinter();
            List<LinterResult> results = linter.Lint(doc);
            Assert.AreEqual(1, results.Count);
            LinterResult warning = results[0];
            Assert.AreEqual("The marker cheese is unknown", warning.Message);
            Assert.AreEqual(16, warning.Position);
        }


    }
}