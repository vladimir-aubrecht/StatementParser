
using System;
using CsvHelper.Configuration.Attributes;

namespace StatementParser.Parsers.Brokers.Lynx.CsvModels
{
	internal class WithholdingTaxRowModel
	{
		[Index(0)]
		public string Section { get; set; }

		[Index(2)]
		public string Currency { get; set; }

		[Index(3)]
		public DateTime? Date { get; set; }

		[Index(4)]
		public string Description { get; set; }

		[Index(5)]
		public decimal Amount { get; set; }

		[Index(6)]
		public string Code { get; set; }

		public override string ToString()
		{
			return $"{nameof(Section)}: {Section} {nameof(Currency)}: {Currency} {nameof(Date)}: {Date} {nameof(Description)}: {Description} {nameof(Amount)}: {Amount} {nameof(Code)}: {Code}";
		}
	}
}