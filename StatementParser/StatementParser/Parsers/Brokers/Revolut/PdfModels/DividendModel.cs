using System;
using ASoft.TextDeserializer.Attributes;

namespace StatementParser.Parsers.Brokers.Revolut.PdfModels
{
    [DeserializeByRegex("(?<Date>\\d{4}-\\d{2}-\\d{2})(?<Symbol>[A-Z]+)(?<SecurityName>[A-Z][^0-9]*?)(?<ISIN>US.{10})(?<Country>.{2})U?S?\\$(?<Dividend>[^\\$]+?)U?S?\\$(?<WithholdingTax>[^\\$]+?)U?S?\\$(?<Amount>[^\\$]+)")]
	internal class DividendModel
	{
		public DateTime Date { get; set; }

        public string Symbol { get; set; }

        public string SecurityName { get; set; }

        public string ISIN { get; set; }

        public string Country { get; set; }

        public string Dividend { get; set; }

        public string WithholdingTax { get; set; }

        public string Amount { get; set; }
	}
}
