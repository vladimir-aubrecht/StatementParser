using StatementParser.Parsers.Pdf.Attributes;

namespace StatementParser.Parsers.Brokers.Fidelity.PdfModels
{
    [DeserializeByRegex("(?<Date>[0-9]{2}/[0-9]{2}) (?<Name>.+?)  ?[0-9]+ Dividend Received  --\\$?(?<Income>[0-9]+\\.[0-9]{2})")]
    internal class ActivityDividendModel
    {
        public string Date { get; set; }
        public string Name { get; set; }
        public decimal Income { get; set; }
    }
}
