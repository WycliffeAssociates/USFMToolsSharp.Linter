using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using USFMToolsSharp.Linter.LinterModules;
using USFMToolsSharp.Linter.Models;
using USFMToolsSharp.LinterModules;
using USFMToolsSharp.Models.Markers;

namespace USFMToolsSharp.Linter
{
    public class USFMLinter
    {
        public List<ILinterModule> linters = new List<ILinterModule>() {
            new FindUnknownMarkers(),
            new VerseMarkerValidation(),
            new MissingEndMarkers(),
            new UnpairedEndMarkers(),
            new MissingTableRows(),
            // Check null/empty marker properties
            new MissingCAMarkerProps(),
            new MissingCLMarkerProps(),
            new MissingCMarkerProps(),
            new MissingCPMarkerProps(),
            new MissingFKMarkerProps(),
            new MissingFMarkerProps(),
            new MissingFRMarkerProps(),
            new MissingFVMarkerProps(),
            new MissingHMarkerProps(),
            new MissingIDEMarkerProps(),
            new MissingIDMarkerProps(),
            new MissingIMTMarkerProps(),
            new MissingIOTMarkerProps(),
            new MissingISMarkerProps(),
            new MissingMRMarkerProps(),
            new MissingMSMarkerProps(),
            new MissingMTMarkerProps(),
            new MissingQACMarkerProps(),
            new MissingQAMarkerProps(),
            new MissingREMMarkerProps(),
            new MissingSMarkerProps(),
            new MissingSPMarkerProps(),
            new MissingSTSMarkerProps(),
            new MissingTOC1MarkerProps(),
            new MissingTOC2MarkerProps(),
            new MissingTOC3MarkerProps(),
            new MissingTOCA1MarkerProps(),
            new MissingTOCA2MarkerProps(),
            new MissingTOCA3MarkerProps(),
            new MissingUSFMMarkerProps(),
            new MissingVAMarkerProps(),
            new MissingVPMarkerProps(),
            new MissingWMarkerProps(),
            new MissingXMarkerProps(),
            new MissingXOMarkerProps(),

        };
        public List<LinterResult> Lint(USFMDocument input)
        {
            List<LinterResult> output = new List<LinterResult>();
            foreach (var linter in linters)
            {
                output.AddRange(linter.Lint(input));
            }
            return output;
        }

        public async Task<List<LinterResult>> LintAsync(USFMDocument input)
        {
            List<LinterResult> output = new List<LinterResult>();
            output = await PerformLintParallelAsync(input);
            return output;
        }

        private async Task<List<LinterResult>> PerformLintParallelAsync(USFMDocument root)
        {
            List<LinterResult> output = new List<LinterResult>();
            List<Task<List<LinterResult>>> tasks = new List<Task<List<LinterResult>>>();
            foreach (var linter in linters)
            {
                tasks.Add(Task.Run(() => linter.Lint(root)));
            }
            var results = await Task.WhenAll(tasks);
            foreach (var item in results)
            {
                output.AddRange(item);
            }
            return output;
        }
    }
}
