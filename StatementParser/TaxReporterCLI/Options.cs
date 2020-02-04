using Commander.NET.Attributes;

namespace TaxReporterCLI
{
    internal class Options
    {
        [Parameter("j", "json", Required = Required.No, Description = "Switch output format into json.")]
        public bool ShouldPrintAsJson { get; set; } = false;

        [Parameter("x", "excelSheetPath", Required = Required.No, Description = "Absolute or relative path to file where excel sheet will be stored.")]
        public string ExcelSheetPath { get; set; } = null;

        [PositionalParameter(0, "inputStatementFilePath", Required = Required.Yes, Description = "Relative or absolute path to statement file or multiple paths each as one argument.")]
        [PositionalParameterList]
        public string[] StatementFilePaths { get; set; }
    }
}