using System;
using StatementParser.Parsers.Pdf;

namespace StatementParser.Parsers.Brokers.Fidelity.PdfRows
{
    [DeserializeByRegex("(?<Date>[0-9]{2}/[0-9]{2}) (?<Name>.+?) SHARES DEPOSITED VALUE OF TRANSACTION .+?Conversion (?<Amount>[0-9]+\\.[0-9]{3})\\$?(?<Price>[0-9]+\\.[0-9]{4})")]
    internal class ActivityOtherRow
    {
        public string Date { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
    }
}
