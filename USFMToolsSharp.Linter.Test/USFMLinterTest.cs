using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using USFMToolsSharp.Linter.Models;
using USFMToolsSharp.Models.Markers;

namespace USFMToolsSharp.Linter.Test
{

    [TestClass]
    public class USFMLinterTest
    {

        List<string> pairedMarkers = new List<string>() {
                "add",
                "bd",
                "bdit",
                "bk",
                "ca",
                "em",
                "fv",
                "ior",
                "it",
                "nd",
                "no",
                "qac",
                "qs",
                "rq",
                "sc",
                "sup",
                "tl",
                "va",
                "vp",
                "w",
                "x"
            };



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
        public void TestValidVerseMarker()
        {
            USFMParser parser = new();
            USFMDocument doc = parser.ParseFromString("\\v 1 \\v 2 \\v 1-23");
            USFMLinter linter = new USFMLinter();
            List<LinterResult> results = linter.Lint(doc);

            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestZeroVerseRange()
        {
            USFMParser parser = new();
            USFMDocument doc = parser.ParseFromString("spacing before \\v 0");
            USFMLinter linter = new USFMLinter();
            List<LinterResult> results = linter.Lint(doc);

            Assert.AreEqual(1, results.Count);

            LinterResult warning = results[0];
            Assert.AreEqual("Verse number is invalid", warning.Message);
            Assert.AreEqual(15, warning.Position);
        }

        [TestMethod]
        public void TestInvalidDoubleVerseRange()
        {
            USFMParser parser = new();
            USFMDocument doc = parser.ParseFromString("spacing before \\v 1-2-3");
            USFMLinter linter = new USFMLinter();
            List<LinterResult> results = linter.Lint(doc);

            Assert.AreEqual(1, results.Count);

            LinterResult warning = results[0];
            Assert.AreEqual("Verse number is invalid", warning.Message);
            Assert.AreEqual(15, warning.Position);
        }

        [TestMethod]
        public void TestReverseOrderInvalidVerseRange()
        {
            USFMParser parser = new();
            USFMDocument doc = parser.ParseFromString("spacing before \\v 3-1");
            USFMLinter linter = new USFMLinter();
            List<LinterResult> results = linter.Lint(doc);

            Assert.AreEqual(1, results.Count);

            LinterResult warning = results[0];
            Assert.AreEqual("Verse number is invalid", warning.Message);
            Assert.AreEqual(15, warning.Position);
        }

        [TestMethod]
        public void TestMissingVerseMarker1()
        {
            USFMParser parser = new();
            USFMDocument doc = parser.ParseFromString("spacing before \\v invalid");
            USFMLinter linter = new USFMLinter();
            List<LinterResult> results = linter.Lint(doc);

            Assert.AreEqual(1, results.Count);

            LinterResult warning = results[0];
            Assert.AreEqual("Verse number is missing", warning.Message);
            Assert.AreEqual(15, warning.Position);
        }

        [TestMethod]
        public void TestMissingVerseMarker2()
        {
            USFMParser parser = new();
            USFMDocument doc = parser.ParseFromString("spacing before \\v");
            USFMLinter linter = new USFMLinter();
            List<LinterResult> results = linter.Lint(doc);

            Assert.AreEqual(1, results.Count);

            LinterResult warning = results[0];
            Assert.AreEqual("Verse number is missing", warning.Message);
            Assert.AreEqual(15, warning.Position);
        }

        [TestMethod]
        public void TestMissingVerseMarker3()
        {
            USFMParser parser = new();
            USFMDocument doc = parser.ParseFromString("spacing before \\v \\bd stuff \\bd*");
            USFMLinter linter = new USFMLinter();
            List<LinterResult> results = linter.Lint(doc);

            Assert.AreEqual(1, results.Count);

            LinterResult warning = results[0];
            Assert.AreEqual("Verse number is missing", warning.Message);
            Assert.AreEqual(15, warning.Position);
        }

        [TestMethod]
        public void TestMultipleInvalidVerseMarkers()
        {
            USFMParser parser = new();
            USFMDocument doc = parser.ParseFromString("spacing before \\v \\bd stuff \\bd* \\v 1-2-3");
            USFMLinter linter = new USFMLinter();
            List<LinterResult> results = linter.Lint(doc);

            Assert.AreEqual(2, results.Count);

            LinterResult warning0 = results[0];
            Assert.AreEqual("Verse number is missing", warning0.Message);
            Assert.AreEqual(15, warning0.Position);

            LinterResult warning1 = results[1];
            Assert.AreEqual("Verse number is invalid", warning1.Message);
            Assert.AreEqual(33, warning1.Position);
        }



        [TestMethod]
        public void TestUnpairedEndMarkers()
        {
            USFMParser parser = new();

            foreach (string marker in pairedMarkers)
            {
                USFMDocument doc = parser.ParseFromString($"Test unpaired marker \\{marker}*");
                USFMLinter linter = new USFMLinter();
                List<LinterResult> results = linter.Lint(doc);

                Assert.AreEqual(1, results.Count);

                LinterResult warning = results[0];
                Assert.AreEqual($"Missing opening marker for {marker}*", warning.Message);
                Assert.AreEqual(21, warning.Position);
            }
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
            Assert.AreEqual("Missing opening marker for bd*", warning0.Message);
            Assert.AreEqual(17, warning0.Position);

            LinterResult warning1 = results[1];
            Assert.AreEqual("Missing opening marker for add*", warning1.Message);
            Assert.AreEqual(22, warning1.Position);
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
            Assert.AreEqual("Missing opening marker for bd*", warning.Message);
            Assert.AreEqual(22, warning.Position);
        }

        [TestMethod]
        public void TestUnpairedMarkersInPositions()
        {
            USFMParser parser = new();
            USFMDocument doc = parser.ParseFromString("\\v 1 verse one in bold \\bd* with stuff after");
            USFMLinter linter = new USFMLinter();
            List<LinterResult> results = linter.Lint(doc);

            Assert.AreEqual(1, results.Count);

            LinterResult warning = results[0];
            Assert.AreEqual("Missing opening marker for bd*", warning.Message);
            Assert.AreEqual(23, warning.Position);
        }
    }
}