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
            USFMParser parser = new();
            USFMDocument doc = parser.ParseFromString("Text \\sup Missing End Marker");
            USFMLinter linter = new USFMLinter();
            List<LinterResult> results = linter.Lint(doc);

            Assert.AreEqual(1, results.Count);

            LinterResult warning = results[0];
            Assert.AreEqual("Missing closing marker for sup", warning.Message);
            Assert.AreEqual(5, warning.Position);
        }





    }
}