
using System;
using CsvHelper.Configuration.Attributes;

namespace StatementParser.Parsers.Brokers.Lynx.CsvModels
{
	internal class DividendsRowModel
	{
		[Name("Dividends")]
		public string Section { get; set; }

		[Name("Currency")]
		public string Currency { get; set; }

		[Name("Date")]
		public DateTime? Date { get; set; }

		[Name("Description")]
		public string Description { get; set; }

		[Name("Amount")]
		public decimal Amount { get; set; }

		public override string ToString()
		{
			return $"{nameof(Section)}: {Section} {nameof(Currency)}: {Currency} {nameof(Date)}: {Date} {nameof(Description)}: {Description} {nameof(Amount)}: {Amount}";
		}
	}
}