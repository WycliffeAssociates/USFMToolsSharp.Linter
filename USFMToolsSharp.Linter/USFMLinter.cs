﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using USFMToolsSharp.Linter.LinterModules;
using USFMToolsSharp.Linter.Models;
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
            new MissingTableRows()
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
