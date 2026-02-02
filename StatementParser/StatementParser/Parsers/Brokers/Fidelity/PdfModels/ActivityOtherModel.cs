using ASoft.TextDeserializer.Attributes;

namespace StatementParser.Parsers.Brokers.Fidelity.PdfModels
{
	[DeserializeByRegex("t?(?<Date>[0-9]{2}/[0-9]{2}) (?<Name>.+?) SHARES DEPOSITED((?! VALUE OF TRANSACTION).)* .+?Conversion (?<Amount>[0-9]+\\.[0-9]{3})\\$?(?<Price>[0-9]+\\.[0-9]{4})")]
	internal class ActivityOtherModel
	{
		public string Date { get; set; }
		public string Name { get; set; }
		public decimal Amount { get; set; }
		public decimal Price { get; set; }
	}
}
