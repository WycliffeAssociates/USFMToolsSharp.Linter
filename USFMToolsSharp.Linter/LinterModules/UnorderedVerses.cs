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
            List<LinterResult> results = new List<LinterResult>();

            foreach (Marker marker in input.Contents)
            {
                if (marker is CMarker)
                {
                    results.AddRange(ValidateVerseOrder((CMarker)marker));
                }
            }
            return results;
        }
        public List<LinterResult> ValidateVerseOrder(CMarker input)
        {
            List<int> verseNumbers = new List<int> { 0 };
            List<LinterResult> results = new List<LinterResult>();
            foreach(VMarker verse in input.GetChildMarkers<VMarker>())
            {
                if (string.IsNullOrEmpty(verse.StartingVerse.ToString()))
                {
                    if (verseNumbers.Count > 0)
                    {
                        if (verse.StartingVerse != verseNumbers[verseNumbers.Count - 1] + 1)
                        {
                            results.Add(new LinterResult
                            {
                                Position = verse.Position,
                                Level = LinterLevel.Warning,
                                Message = $"Chapter {input.Number}: Verse {verse.StartingVerse} is not in the correct order."
                            });
                        }
                    }
                    verseNumbers.Add(verse.EndingVerse);

                }
                else
                {
                    if (verseNumbers.Count > 0)
                    {
                        if (Int32.Parse(verse.VerseNumber) != verseNumbers[verseNumbers.Count - 1] + 1)
                        {
                            results.Add(new LinterResult
                            {
                                Position = verse.Position,
                                Level = LinterLevel.Warning,
                                Message = $"Chapter {input.Number}: Verse {verse.VerseNumber} is not in the correct order."
                            });
                        }
                    }
                    verseNumbers.Add(Int32.Parse(verse.VerseNumber));
                }
            }
            return results;
        }

    }
}