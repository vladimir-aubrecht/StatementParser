using System.ComponentModel;
using Commander.NET.Attributes;

namespace StatementParserCLI
{
    internal class Options
    {
        [Parameter("j", "json", Description = "Switch output format into json.")]
        public bool ShouldPrintAsJson { get; set; } = false;

        [PositionalParameter(0, "inputStatementFilePath", Description = "Relative or absolute path to statement file or multiple paths each as one argument.")]
        [PositionalParameterList]
        public string[] StatementFilePaths { get; set; }

    }
}