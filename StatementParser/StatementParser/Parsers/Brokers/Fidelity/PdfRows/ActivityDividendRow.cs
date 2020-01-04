using System;
using StatementParser.Parsers.Pdf;

namespace StatementParser.Parsers.Brokers.Fidelity.PdfRows
{
    [DeserializeByRegex("(?<Date>[0-9]{2}/[0-9]{2}) (?<Name>.+?)  ?[0-9]+ Dividend Received  --\\$?(?<Income>[0-9]+\\.[0-9]{2})")]
    internal class ActivityDividendRow
    {
        public string Date { get; set; }
        public string Name { get; set; }
        public string Income { get; set; }
    }
}
