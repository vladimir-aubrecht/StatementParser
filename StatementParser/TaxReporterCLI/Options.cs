using Commander.NET.Attributes;

namespace TaxReporterCLI
{
    internal class Options
    {
        [Parameter("j", "json", Required = Required.No, Description = "Switch output format into json.")]
        public bool ShouldPrintAsJson { get; set; } = false;

        [Parameter("x", "excelSheetPath", Required = Required.No, Description = "Absolute or relative path to file where excel sheet will be stored.")]
        public string ExcelSheetPath { get; set; } = null;

        [Parameter("r", "overrideYearlyExchangeRate", Required = Required.No, Description = "Yearly exchange rate which should be used instead of downloaded one. This is override useful in case yearly exchange rate is not published to proper place yet. Use json format: {'usd' = 23.0, 'czk' = 0.5}.")]
        public string OverrideYearlyExchangeRate { get; set; } = null;

        [PositionalParameter(0, "inputStatementFilePath", Required = Required.Yes, Description = "Relative or absolute path to statement file or multiple paths each as one argument.")]
        [PositionalParameterList]
        public string[] StatementFilePaths { get; set; }
    }
}