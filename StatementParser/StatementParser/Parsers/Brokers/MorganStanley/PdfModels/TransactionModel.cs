using System;
using ASoft.TextDeserializer.Attributes;

namespace StatementParser.Parsers.Brokers.MorganStanley.PdfModels
{
	[DeserializeByRegex("(?<Date>[0-9]{2}/[0-9]{2}/[0-9]{2})(?<Type>Share Deposit|Dividend Credit|Withholding Tax|Dividend Reinvested|Sale|Proceeds Disbursement) +(?<Quantity>[0-9]+\\.[0-9]{3})? +\\$?(?<Price>[0-9]+\\.[0-9]{4})? +\\$?(?<GrossAmount>[0-9]+\\.[0-9]{2})? +\\$?(?<Commissions>[0-9]+\\.[0-9]{2})? +\\$?(?<DistributionFees>[0-9]+\\.[0-9]{2})? +\\$?(?<OtherCosts>[0-9]+\\.[0-9]{2})? +\\$?(?<NetAmountRaw>\\(?[0-9]+\\.[0-9]{2}\\)?)?")]
	internal class TransactionModel
	{
		public DateTime Date { get; set; }

		public string Type { get; set; }

		public decimal Quantity { get; set; }

		public decimal Price { get; set; }

		public decimal GrossAmount { get; set; }

		public decimal Commissions { get; set; }

		public decimal DistributionFees { get; set; }

		public decimal OtherCosts { get; set; }

		public string NetAmountRaw { get; set; }

		public decimal NetAmount => Convert.ToDecimal(NetAmountRaw?.Replace('(', '-').TrimEnd(')'));

        public override string ToString()
		{
			return $"{nameof(Date)}: {Date} {nameof(Type)}: {Type} {nameof(Quantity)}: {Quantity} {nameof(Price)}: {Price} {nameof(GrossAmount)}: {GrossAmount} {nameof(Commissions)}: {Commissions} {nameof(DistributionFees)}: {DistributionFees} {nameof(OtherCosts)}: {OtherCosts} {nameof(NetAmount)}: {NetAmount}";
		}
	}
}
