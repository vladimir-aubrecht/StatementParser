using System;
using System.Globalization;

using ASoft.TextDeserializer.Attributes;

namespace StatementParser.Parsers.Brokers.MorganStanley.PdfModels
{
	[DeserializeByRegex(
		@"^(?<Date>[0-9]{1,2}/[0-9]{1,2}/[0-9]{2})" +
		@"\t(?<Type>Share Deposit|Dividend Credit|Withholding Tax|Dividend Reinvested|Sale|Proceeds Disbursement)" +
		@"(?:" +
			@"\t(?<Quantity>[0-9,]+\.[0-9]{3})" +
			@"\t\$?(?<Price>[0-9,]+\.[0-9]{4})" +
		@")?" +
		@"(?:" +
			@"(?:\t\$?(?<GrossAmount>[0-9,]+\.[0-9]{2}))?" +
			@"(?:\t\$?(?<TotalTaxesAndFees>[0-9,]+\.[0-9]{2}))?" +
			@"\t\$?(?<TotalNetAmountRaw>\(?[0-9,]+\.[0-9]{2})\)?" +
		@")?$")]
	internal class TransactionModel2022
	{
		public DateTime Date { get; set; }

		public string Type { get; set; }

		public decimal Quantity { get; set; }

		public decimal Price { get; set; }

		public decimal GrossAmount { get; set; }

		public decimal TotalTaxesAndFees { get; set; }

		public string TotalNetAmountRaw { get; set; }

		public decimal TotalNetAmount => Convert.ToDecimal(TotalNetAmountRaw?.Replace('(', '-').TrimEnd(')'), new CultureInfo("en-US"));

		public override string ToString()
		{
			return $"{nameof(Date)}: {Date} {nameof(Type)}: {Type} {nameof(Quantity)}: {Quantity} {nameof(Price)}: {Price} {nameof(GrossAmount)}: {GrossAmount} {nameof(TotalTaxesAndFees)}: {TotalTaxesAndFees} {nameof(TotalNetAmount)}: {TotalNetAmount}";
		}
	}
}
