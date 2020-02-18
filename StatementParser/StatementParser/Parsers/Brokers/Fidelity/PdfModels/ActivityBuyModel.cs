using StatementParser.Parsers.Pdf.Attributes;

namespace StatementParser.Parsers.Brokers.Fidelity.PdfModels
{
	[DeserializeByRegex("(?<Date>[0-9]{2}/[0-9]{2}) (?<Name>.+?) ESPP.+You Bought (?<Amount>[0-9]+\\.[0-9]{3})\\$(?<Price>[0-9]+\\.[0-9]{5})")]
	internal class ActivityBuyModel
	{
		public string Date { get; set; }

		public string Name { get; set; }

		public decimal Amount { get; set; }

		public decimal Price { get; set; }
	}
}
