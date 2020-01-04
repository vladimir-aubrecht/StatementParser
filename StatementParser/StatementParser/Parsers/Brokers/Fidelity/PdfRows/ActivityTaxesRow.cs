using System;
using StatementParser.Parsers.Pdf;

namespace StatementParser.Parsers.Brokers.Fidelity.PdfRows
{
    [DeserializeByRegex(
        deserializationRegexPattern: "(?<Date>[0-9]{2}/[0-9]{2}) .+?Tax -\\$(?<Tax>[0-9]+\\.[0-9]{2})",
        pageBodyRegexPattern: "STOCK PLAN ACCOUNTActivityTaxes Withheld DateSecurityDescriptionAmount(.+?)Total Federal Taxes Withheld",
        rowSplitRegexPattern: "([0-9]{2}/[0-9]{2} )")]
    internal class ActivityTaxesRow
    {
        public string Date { get; set; }

        public decimal Tax { get; set; }
    }
}
