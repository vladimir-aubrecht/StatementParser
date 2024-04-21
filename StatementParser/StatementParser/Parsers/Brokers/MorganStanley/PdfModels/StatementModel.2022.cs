using ASoft.TextDeserializer.Attributes;

namespace StatementParser.Parsers.Brokers.MorganStanley.PdfModels
{
	[DeserializeByRegex(@"Issuer Description:\t(?<Name>[^\t\n]+)")]
	internal class StatementModel2022
	{
		public string Name { get; set; }

		[DeserializeCollectionByRegex(
			@"([0-9]{1,2}/[0-9]{1,2}/[0-9]{2})",
			@"\nGross\nTransaction Date\tActivity Type\tQuantity\tPrice\tAmount\tTotal Taxes and Fees\tTotal Net Amount\n((?s).+?)\nSell Transactions are provided as of trade date\.")]
		public TransactionModel2022[] Transactions { get; set; }
	}
}
