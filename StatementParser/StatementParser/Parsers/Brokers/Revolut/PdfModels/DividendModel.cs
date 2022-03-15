using System;
using ASoft.TextDeserializer.Attributes;

namespace StatementParser.Parsers.Brokers.Revolut.PdfModels
{
    [DeserializeByRegex("(?<Date>\\d{4}-\\d{2}-\\d{2})(?<Symbol>[^\\$]+)\\$(?<Dividend>[^\\$]+)\\$(?<WithholdingTax>[^\\$]+)\\$(?<Amount>[^\\$]+)")]
	internal class DividendModel
	{
		public DateTime Date { get; set; }

        public string Symbol { get; set; }

        public decimal Dividend { get; set; }

        public decimal WithholdingTax { get; set; }

        public decimal Amount { get; set; }
	}
}
