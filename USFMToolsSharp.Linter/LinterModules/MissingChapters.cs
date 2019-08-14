using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using USFMToolsSharp.Linter.Models;
using USFMToolsSharp.Models.Markers;
using System.IO;
using Newtonsoft.Json.Linq;

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
            Dictionary<string, int> chapterCounts = populateBookData();
            List<Marker> chapters = new List<Marker>();

            string currentID ="";
            int chapterMaxCount = -1;


            foreach (Marker marker in input.Contents)
            {
                if(marker is IDMarker)
                {
                    currentID = ((IDMarker)marker).TextIdentifier.Split(' ')[0].ToUpper();
                    if (chapterMaxCount > 0)
                    {
                        if(chapters.Count != chapterMaxCount)
                        {
                            results.Add(new LinterResult
                            {
                                Position = chapters[chapters.Count-1].Position,
                                Level = LinterLevel.Warning,
                                Message = $"Chapter count is a little off in the book of {currentID}."
                            });
                        }
                    }
                    
                    if(chapterCounts.ContainsKey(currentID))
                    {
                        chapters.Clear();
                        chapterMaxCount = chapterCounts[currentID];
                    }                    
                }
                if(marker is CMarker)
                {
                    chapters.Add(marker);
                }

            }
            if (chapters.Count != chapterMaxCount)
            {
                results.Add(new LinterResult
                {
                    Position = chapters[chapters.Count - 1].Position,
                    Level = LinterLevel.Warning,
                    Message = $"Chapter count is a little off in the book of {currentID}."
                });
            }

            return results;
        }
        /// <summary>
        /// Extracts the Book ID (Abbreviation) and Total Number of Chapters into a Dictionary
        /// </summary>
        /// <returns></returns>
        private Dictionary<string,int> populateBookData()
        {
            JObject data = retrieveBibleMetadata();
            Dictionary<string, int> output = new Dictionary<string, int>();
            foreach (JProperty book in data.Properties())
            {
                output[book.Name] = Int32.Parse(data[book.Name]["TotalChapters"].ToString());
            }
            return output;
        }
        /// <summary>
        /// Retrieves Bible metadata from local json file (MetaData.json)
        /// </summary>
        /// <returns></returns>
        private JObject retrieveBibleMetadata()
        {
            string jsonData = File.ReadAllText(@"./MetaData.json");
            return JObject.Parse(jsonData);
            
        }
    }
}
