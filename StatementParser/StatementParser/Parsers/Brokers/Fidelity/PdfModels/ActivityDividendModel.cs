using System;
using StatementParser.Parsers.Pdf;

namespace StatementParser.Parsers.Brokers.Fidelity.PdfModels
{
    [DeserializeByRegex(
        deserializationRegexPattern: "(?<Date>[0-9]{2}/[0-9]{2}) (?<Name>.+?)  ?[0-9]+ Dividend Received  --\\$?(?<Income>[0-9]+\\.[0-9]{2})",
        pageBodyRegexPattern: "Dividends, Interest & Other Income \\(Includes dividend reinvestment\\)SettlementDateSecurity NameSymbol/CUSIPDescriptionQuantityPriceAmount(.+?)Total Dividends, Interest & Other Income",
        rowSplitRegexPattern: "([0-9]{2}/[0-9]{2} )")]
    internal class ActivityDividendModel
    {
        public string Date { get; set; }
        public string Name { get; set; }
        public decimal Income { get; set; }
    }
}
