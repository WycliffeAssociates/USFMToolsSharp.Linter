using System;
using System.Collections.Generic;
using System.Text;
using USFMToolsSharp.Linter.Models;
using USFMToolsSharp.Models.Markers;

namespace USFMToolsSharp.Linter.LinterModules
{
    public interface ILinterModule
    {
        List<LinterResult> Lint(USFMDocument input);
    }
}
