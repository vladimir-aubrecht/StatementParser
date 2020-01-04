using System;
using StatementParser.Parsers.Pdf;

namespace StatementParser.Parsers.Brokers.Fidelity.PdfModels
{
    [DeserializeByRegex(
        deserializationRegexPattern: "(?<Date>[0-9]{2}/[0-9]{2}) (?<Name>.+?) SHARES DEPOSITED VALUE OF TRANSACTION .+?Conversion (?<Amount>[0-9]+\\.[0-9]{3})\\$?(?<Price>[0-9]+\\.[0-9]{4})",
        pageBodyRegexPattern: "Other Activity In SettlementDateSecurity NameSymbol/CUSIPDescriptionQuantityPriceTransactionCostAmount(.+?)Total Other Activity In",
        rowSplitRegexPattern: "([0-9]{2}/[0-9]{2} )")]
    internal class ActivityOtherModel
    {
        public string Date { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
    }
}
