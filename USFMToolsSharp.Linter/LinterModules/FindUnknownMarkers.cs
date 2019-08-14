using System;
using System.Collections.Generic;
using System.Text;
using USFMToolsSharp.Linter.Models;
using USFMToolsSharp.Models.Markers;

namespace USFMToolsSharp.Linter.LinterModules
{
    public class FindUnknownMarkers : ILinterModule
    {
        public List<LinterResult> Lint(USFMDocument input)
        {
            List<LinterResult> output = new List<LinterResult>();
            foreach(var marker in input.GetChildMarkers<UnknownMarker>())
            {
                output.Add(new LinterResult()
                {
                    Level = LinterLevel.Warning,
                    Position = marker.Position,
                    Message = $"The marker {marker.ParsedIdentifier} is unknown"
                }
                );
            }
            return output;
        }
    }
}
