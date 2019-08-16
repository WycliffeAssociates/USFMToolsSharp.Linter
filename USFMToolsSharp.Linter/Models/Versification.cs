using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace USFMToolsSharp.Linter.Models
{
    public class Versisification
    {

        public Dictionary<string, Book> metadata { get; set; }

        public class Chapter
        {
            public int ChapterNum { get; set; }
            public int TotalVerses { get; set; }

        }
        public class Book
        {
            public List<Chapter> Chapters { get; set; }
            public int BookID { get; set; }
            public string BookName { get; set; }
            public int TotalChapters { get; set; }

        }
        public Versisification(string filename)
        {
            string jsonData = File.ReadAllText(filename);
            metadata = JsonConvert.DeserializeObject<Dictionary<string, Book>>(jsonData);

        }
    }
}
