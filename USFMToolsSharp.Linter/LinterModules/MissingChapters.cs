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

namespace USFMToolsSharp.Linter.LinterModules
{
    /// <summary>
    /// Identifies Missing Chapters in a Book. 
    /// <remark> A new book is defined on ID Markers </remark>
    /// </summary>
    public class MissingChapters : ILinterModule
    {
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
            JObject data = retrieveBibleMetadata(@"./metadata.json");
            Dictionary<string, List<int>> output = new Dictionary<string, List<int>>();
            foreach (JProperty book in data.Properties())
            {
                output[book.Name] = Enumerable.Range(1, Int32.Parse(data[book.Name]["TotalChapters"].ToString())).ToList(); 
            }
            return output;
        }
        /// <summary>
        /// Retrieves Bible metadata from local json file (MetaData.json)
        /// </summary>
        /// <returns></returns>
        private JObject retrieveBibleMetadata(string fileName)
        {
            string jsonData = File.ReadAllText(fileName);
            return JObject.Parse(jsonData);
            
        }
    }
}
