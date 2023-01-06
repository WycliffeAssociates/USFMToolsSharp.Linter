using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using USFMToolsSharp.Linter.Models;
using USFMToolsSharp.Models.Markers;

namespace USFMToolsSharp.Linter.Test
{

    [TestClass]
    public class USFMLinterTestCraig
    {

        [TestMethod]
        public void TestMissingEndMarker()
        {
            List<string> markersToTest = new List<string>() {
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

            USFMParser parser = new();

            foreach (string marker in markersToTest)
            {
                USFMDocument doc = parser.ParseFromString($"Text \\{marker} with no end marker");
                USFMLinter linter = new USFMLinter();
                List<LinterResult> results = linter.Lint(doc);

                Assert.AreEqual(1, results.Count);

                LinterResult warning = results[0];
                Assert.AreEqual($"Missing closing marker for {marker}", warning.Message);
                Assert.AreEqual(5, warning.Position);
            }

        }





    }
}