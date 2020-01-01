using CommandLine;

namespace StatementParser
{
    internal class Options
    {
        [Option('i', "inputStatementFilePath", Required = true, HelpText = "Relative or absolute path to statement file.")]
        public string StatementFilePath { get; set; }

        [Option('j', "json", Required = false, HelpText = "Switch output format into json.")]
        public bool ShouldPrintAsJson { get; set; }
    }
}