using System;
using CsvHelper.Configuration.Attributes;
using StatementParser.Models;

namespace StatementParser.Parsers.Brokers.Revolut.CsvModels
{
	internal class AccountRowModel
	{
		[Index(0)]
		public DateTime Date { get; set; }

		[Index(1)]
		public string Ticker { get; set; }

		[Index(2)]
		public string Type { get; set; }

		[Index(3)]
		public decimal? Quantity { get; set; }

		[Index(4)]
		public decimal? PricePerShare { get; set; }

		[Index(5)]
		public decimal? TotalAmount { get; set; }

		[Index(6)]
		public Currency Currency { get; set; }

		[Index(7)]
		public double FxRate { get; set; }

		public override string ToString()
		{
			var dateString = Date.ToShortDateString();
			return $"{nameof(Date)}: {dateString} {nameof(Ticker)}: {Ticker} {nameof(Type)}: {Type} {nameof(Quantity)}: {Quantity} {nameof(PricePerShare)}: {PricePerShare} {nameof(TotalAmount)}: {TotalAmount} {nameof(Currency)}: {Currency} {nameof(FxRate)}: {FxRate}";
		}
	}
}