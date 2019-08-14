using System;
using System.Collections.Generic;
using System.Text;
using USFMToolsSharp.Linter.LinterModules;
using USFMToolsSharp.Linter.Models;
using USFMToolsSharp.Models.Markers;

namespace USFMToolsSharp.Linter.LinterModules
{
    public class UnorderedChapters : ILinterModule
    {
        public List<LinterResult> Lint(USFMDocument input)
        {
            List<int> chapterIndecies = new List<int>();
            List<LinterResult> results = new List<LinterResult>();
            foreach (Marker marker in input.Contents)
            {
                if (marker is CMarker)
                {
                    int chapIndex = ((CMarker)marker).Number;
                    if (chapterIndecies.Count != 0)
                    {
                        if (chapIndex != chapterIndecies[chapterIndecies.Count - 1] + 1)
                        {
                            results.Add(new LinterResult
                            {
                                Position = marker.Position,
                                Level = LinterLevel.Error,
                                Message = $"Chapter {chapIndex} is not in the correct order."
                            });
                        }
                    }
                    
                    chapterIndecies.Add(((CMarker)marker).Number);
                }
                if (marker is IDMarker)
                {
                    chapterIndecies.Clear();
                }
            }
            return results;
        }
    }
}