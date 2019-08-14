using System;
using System.Collections.Generic;
using System.Text;
using USFMToolsSharp.Linter.LinterModules;
using USFMToolsSharp.Linter.Models;
using USFMToolsSharp.Models.Markers;

namespace USFMToolsSharp.Linter.LinterModules
{
    public class UnorderedVerses : ILinterModule
    {
        public List<LinterResult> Lint(USFMDocument input)
        {
            List<int> chapterIndecies = new List<int>();
            List<LinterResult> results = new List<LinterResult>();

            foreach (Marker marker in input.Contents)
            {
                if (marker is CMarker)
                {
                    results.AddRange(ValidateVerseOrder(marker));
                }
            }
            return results;
        }
        public List<LinterResult> ValidateVerseOrder(Marker input)
        {
            List<LinterResult> results = new List<LinterResult>();
            return results;
        }
    }
}