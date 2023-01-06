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

        [TestMethod]
        public void TestMultipleUnknownMarkers()
        {
            USFMParser parser = new();
            USFMDocument doc = parser.ParseFromString("Invalid marker 1: \\cheese Invalid marker 2: \\apple");
            USFMLinter linter = new USFMLinter();
            List<LinterResult> results = linter.Lint(doc);

            Assert.AreEqual(2, results.Count);

            LinterResult warning0 = results[0];
            Assert.AreEqual("The marker cheese is unknown", warning0.Message);
            Assert.AreEqual(18, warning0.Position);

            LinterResult warning1 = results[1];
            Assert.AreEqual("The marker apple is unknown", warning1.Message);
            Assert.AreEqual(44, warning1.Position);
        }

        [TestMethod]
        public void TestPairedEndMarker()
        {
            USFMParser parser = new();
            USFMDocument doc = parser.ParseFromString("\\bd This is bold \\bd*");
            USFMLinter linter = new USFMLinter();
            List<LinterResult> results = linter.Lint(doc);

            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestUnpairedEndMarker()
        {
            USFMParser parser = new();
            USFMDocument doc = parser.ParseFromString("This is not bold \\bd*");
            USFMLinter linter = new USFMLinter();
            List<LinterResult> results = linter.Lint(doc);

            Assert.AreEqual(1, results.Count);

            LinterResult warning = results[0];
            Assert.AreEqual("Missing Openning marker for bd*", warning.Message);
            Assert.AreEqual(17, warning.Position);
        }

        [TestMethod]
        public void TestMultipleUnpairedEndMarker()
        {
            USFMParser parser = new();
            USFMDocument doc = parser.ParseFromString("This is not bold \\bd* \\add*");
            USFMLinter linter = new USFMLinter();
            List<LinterResult> results = linter.Lint(doc);

            Assert.AreEqual(2, results.Count);

            LinterResult warning0 = results[0];
            Assert.AreEqual("Missing Openning marker for bd*", warning0.Message);
            Assert.AreEqual(17, warning0.Position);

            LinterResult warning1 = results[1];
            Assert.AreEqual("Missing Openning marker for add*", warning1.Message);
            Assert.AreEqual(23, warning1.Position);
        }

        [TestMethod]
        public void TestUnpairedWithPairedMarkers()
        {
            USFMParser parser = new();
            USFMDocument doc = parser.ParseFromString("\\bd This is bold \\bd* \\bd*");
            USFMLinter linter = new USFMLinter();
            List<LinterResult> results = linter.Lint(doc);

            Assert.AreEqual(1, results.Count);

            LinterResult warning = results[0];
            Assert.AreEqual("Missing Openning marker for bd*", warning.Message);
            Assert.AreEqual(24, warning.Position);
        }
    }
}