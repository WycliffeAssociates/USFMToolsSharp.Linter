using System;
using System.Collections.Generic;
using System.Text;
using USFMToolsSharp.Linter.LinterModules;
using USFMToolsSharp.Linter.Models;
using USFMToolsSharp.Models.Markers;

namespace USFMToolsSharp.Linter
{
    public class USFMLinter
    {
        public Versisification metadata;
        public List<ILinterModule> linters = new List<ILinterModule>() {
            new FindUnknownMarkers(),
            new VerseMarkerValidation(),
            new MissingEndMarkers(),
            new UnpairedEndMarkers(),
            new MissingTableRows()
            
        };
        public USFMLinter(Versisification input)
        {
            metadata = input;
            linters.Add(new MissingChapters(metadata));
        }
        public List<LinterResult> Lint(USFMDocument input)
        {
            List<LinterResult> output = new List<LinterResult>();
            foreach(var linter in linters)
            {
                output.AddRange(linter.Lint(input));
            }

            return output;
        }
    }
}
