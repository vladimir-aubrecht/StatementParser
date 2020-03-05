using System;
using CsvHelper.Configuration.Attributes;
using StatementParser.Models;

namespace StatementParser.Parsers.Brokers.Degiro.CsvModels
{
	internal class StatementRowModel
	{
		[Index(0)]
		public DateTime? Date { get; set; }

		[Index(2)]
		public string Name { get; set; }

		[Index(3)]
		public string ISIN { get; set; }

		[Index(4)]
		public string Description { get; set; }

		[Index(5)]
		public Currency? Currency { get; set; }

		[Index(6)]
		public decimal? Income { get; set; }

		public override string ToString()
		{
			var dateString = Date?.ToShortDateString();
			var incomeString = Income ?? 0;
			return $"{nameof(Date)}: {dateString} {nameof(Name)}: {Name} {nameof(ISIN)}: {ISIN} {nameof(Description)}: {Description} {nameof(Currency)}: {Currency} {nameof(Income)}: {incomeString}";
		}
	}
}