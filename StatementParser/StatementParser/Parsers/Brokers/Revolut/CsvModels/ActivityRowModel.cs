using System;
using CsvHelper.Configuration.Attributes;
using StatementParser.Models;

namespace StatementParser.Parsers.Brokers.Revolut.CsvModels
{
	internal class ActivityRowModel
	{
		[Index(0)]
		public DateTime DateAcquired { get; set; }

		[Index(1)]
		public DateTime DateSold { get; set; }

		[Index(2)]
		public string Symbol { get; set; }

		[Index(3)]
		public decimal Quantity { get; set; }

		[Index(4)]
		public decimal CostBasis { get; set; }

		[Index(5)]
		public decimal Amount { get; set; }

		[Index(6)]
		public decimal RealisedPnL{ get; set; }

		public override string ToString()
		{
			var dateAcquiredString = DateAcquired.ToShortDateString();
			var dateSoldString = DateSold.ToShortDateString();
			return $"{nameof(DateAcquired)}: {dateAcquiredString} {nameof(DateSold)}: {dateSoldString} {nameof(Symbol)}: {Symbol} {nameof(Quantity)}: {Quantity} {nameof(CostBasis)}: {CostBasis} {nameof(Amount)}: {Amount} {nameof(Amount)}: {Amount} {nameof(RealisedPnL)}: {RealisedPnL}";
		}
	}
}