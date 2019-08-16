using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using USFMToolsSharp.Linter.Models;
using USFMToolsSharp.Models.Markers;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Linq;
using static USFMToolsSharp.Linter.Models.Versisification;

namespace USFMToolsSharp.Linter.LinterModules
{
    /// <summary>
    /// Identifies Missing Chapters in a Book. 
    /// <remark> A new book is defined on ID Markers </remark>
    /// </summary>
    public class MissingChapters : ILinterModule
    {
        public Versisification ScriptureConfig;
        public MissingChapters(Versisification input)
        {
            ScriptureConfig = input;
        }
        public List<LinterResult> Lint(USFMDocument input)
        {
            List<LinterResult> results = new List<LinterResult>();
            Dictionary<string, List<int>> chapterCounts = populateBookData();
            List<Marker> chapters = new List<Marker>();
            List<string> bookIDs = new List<string>();

            string currentID ="";


            foreach (Marker marker in input.Contents)
            {
                if(marker is IDMarker)
                {
                    currentID = ((IDMarker)marker).TextIdentifier.Split(' ')[0].ToUpper();
                    bookIDs.Add(currentID);
                }
                if(marker is CMarker)
                {
                    chapters.Add(marker);
                    if (chapterCounts.ContainsKey(currentID))
                    {
                        chapterCounts[currentID].Remove(((CMarker)marker).Number);
                    }
                }

            }
            foreach(KeyValuePair<string, List<int>> entry in chapterCounts)
            {
                if (bookIDs.Contains(entry.Key))
                {
                    foreach (int missingChapter in entry.Value)
                    {
                        results.Add(new LinterResult
                        {
                            Position = chapters[chapters.Count - 1].Position,
                            Level = LinterLevel.Warning,
                            Message = $"Missing Chapter {missingChapter} in the book of {entry.Key}."
                        });

                    }
                }
                
            }

            return results;
        }
        /// <summary>
        /// Extracts the Book ID (Abbreviation) and Total Number of Chapters into a Dictionary
        /// </summary>
        /// <returns></returns>
        private Dictionary<string,List<int>> populateBookData()
        {
            Dictionary<string, List<int>> output = new Dictionary<string, List<int>>();
            foreach (KeyValuePair<string,Book> book in ScriptureConfig.metadata)
            {
                output[book.Key] = Enumerable.Range(1, book.Value.TotalChapters).ToList(); 
            }
            return output;
        }

    }
}
