using System;
using StatementParser.Parsers.Pdf;

namespace StatementParser.Parsers.Brokers.Fidelity.PdfRows
{
    [DeserializeByRegex(
        deserializationRegexPattern: "(?<Date>[0-9]{2}/[0-9]{2}/[0-9]{4})\\$(?<PurchasePrice>[0-9]+\\.[0-9]{5})\\$(?<MarketPrice>[0-9]+\\.[0-9]{3})(?<Amount>[0-9]+\\.[0-9]{3})",
        pageBodyRegexPattern: "Employee Stock Purchase SummaryOffering PeriodDescriptionPurchaseDatePurchase PricePurchase DateFair Market ValueSharesPurchasedGain fromPurchase (.+?)Total for all Offering Periods",
        rowSplitRegexPattern: "([0-9]{2}/[0-9]{2}/[0-9]{4}-[0-9]{2}/[0-9]{2}/[0-9]{4})")]
    internal class DiscountedBuyRow
    {
        public DateTime Date { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal MarketPrice { get; set; }
        public decimal Amount { get; set; }
    }
}
