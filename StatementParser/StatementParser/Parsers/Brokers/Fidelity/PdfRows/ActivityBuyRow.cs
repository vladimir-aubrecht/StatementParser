using System;
using StatementParser.Parsers.Pdf;

namespace StatementParser.Parsers.Brokers.Fidelity.PdfRows
{
    [DeserializeByRegex("[0-9]{2}/[0-9]{2} (?<Name>.+?) ESPP.+You Bought (?<Amount>[0-9]+\\.[0-9]{3})\\$(?<Price>[0-9]+\\.[0-9]{5})")]
    internal class ActivityBuyRow
    {
        public string Name { get; set; }

        public string Amount { get; set; }

        public string Price { get; set; }
    }
}
