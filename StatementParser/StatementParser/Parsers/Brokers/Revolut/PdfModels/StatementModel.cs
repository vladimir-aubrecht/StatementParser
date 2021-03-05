using ASoft.TextDeserializer.Attributes;

namespace StatementParser.Parsers.Brokers.Revolut.PdfModels
{
    [DeserializeByRegex("(?<AccountNumber>[^ ]+)Account Name")]
    internal class StatementModel
    {
        public string AccountNumber { get; set; }

        [DeserializeCollectionByRegex(
            "([0-9]{2}/[0-9]{2}/[0-9]{4}[0-9]{2}/[0-9]{2}/[0-9]{4})",
            "ACTIVITYTrade DateSettle DateCurrencyActivity TypeSymbol / DescriptionQuantityPriceAmount(.+)Page \\d+ of \\d+")]
        public ActivityDividendModel[] ActivityDividend { get; set; }
    }
}
