using System;
using System.Collections.Generic;
using System.Text;

namespace USFMToolsSharp.Linter.Models
{
    class Versisification
    {
        public class Chapter
        {
            public int ChapterNum { get; set; }
            public int VerseTotal { get; set; }

        }
        public class Book
        {
            public List<Chapter> Chapters { get; set; }
            public string BookID { get; set; }
            public string BookName { get; set; }
            public int TotalChapters { get; set; }
        }
    }
}
