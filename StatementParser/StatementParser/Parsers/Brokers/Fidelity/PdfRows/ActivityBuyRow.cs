using System;
using StatementParser.Parsers.Pdf;

namespace StatementParser.Parsers.Brokers.Fidelity.PdfRows
{
    [DeserializeByRegex(
        deserializationRegexPattern: "(?<Date>[0-9]{2}/[0-9]{2}) (?<Name>.+?) ESPP.+You Bought (?<Amount>[0-9]+\\.[0-9]{3})\\$(?<Price>[0-9]+\\.[0-9]{5})",
        pageBodyRegexPattern: "Securities Bought & SoldSettlementDateSecurity NameSymbol/CUSIPDescriptionQuantityPriceTransactionCostAmounti(.+?)Total Securities Bought",
        rowSplitRegexPattern: "([0-9]{2}/[0-9]{2} )")]
    internal class ActivityBuyRow
    {
        public string Date { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }

        public decimal Price { get; set; }
    }
}
