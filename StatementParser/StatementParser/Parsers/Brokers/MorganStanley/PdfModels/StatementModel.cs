
using ASoft.TextDeserializer.Attributes;

namespace StatementParser.Parsers.Brokers.MorganStanley.PdfModels
{
	[DeserializeByRegex("Security Name: (?<Name>[^©]+?) *©")]
	internal class StatementModel
	{
		public string Name { get; set; }

		[DeserializeCollectionByRegex(
			"([0-9]{2}/[0-9]{2}/[0-9]{2})",
			"Transaction DateActivity TypeQuantityPriceGross AmountCommissionDistr FeeOther CostsNet Amount(.+?)(:?\\(continued\\)|COMPANY MESSAGE)")]
		public TransactionModel[] Transactions { get; set; }
	}
}
