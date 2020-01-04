using System;
using StatementParser.Parsers.Pdf;

namespace StatementParser.Parsers.Brokers.Fidelity.PdfRows
{
    [DeserializeByRegex("(?<Date>[0-9]{2}/[0-9]{2}/[0-9]{4})\\$(?<PurchasePrice>[0-9]+\\.[0-9]{5})\\$(?<MarketPrice>[0-9]+\\.[0-9]{3})(?<Amount>[0-9]+\\.[0-9]{3})")]
    internal class DiscountedBuyRow
    {
        public string Date { get; set; }
        public string PurchasePrice { get; set; }
        public string MarketPrice { get; set; }
        public string Amount { get; set; }
    }
}
