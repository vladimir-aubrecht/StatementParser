using System;
using TaxReporterCLI.Models.Attributes;

namespace StatementParser.Models
{
	public class ESPPTransaction : Transaction
	{
		[Description("Purchase Price")]
		public decimal PurchasePrice { get; }

		[Description("Market Price")]
		public decimal MarketPrice { get; }
		public decimal Amount { get; }
		public decimal Profit { get; }

		[Description("Total Profit")]
		public decimal TotalProfit { get; }

		public ESPPTransaction(ESPPTransaction eSPPTransaction) : this(eSPPTransaction.Broker, eSPPTransaction.Date, eSPPTransaction.Name, eSPPTransaction.Currency, eSPPTransaction.PurchasePrice, eSPPTransaction.MarketPrice, eSPPTransaction.Amount)
		{

		}

		public ESPPTransaction(Broker broker, DateTime date, string name, Currency currency, decimal purchasePrice, decimal marketPrice, decimal amount) : base(broker, date, name, currency)
		{
			this.PurchasePrice = purchasePrice;
			this.MarketPrice = marketPrice;
			this.Amount = amount;
			this.Profit = marketPrice - purchasePrice;
			this.TotalProfit = Profit * amount;
		}

		public override string ToString()
		{
			return $"{base.ToString()} {nameof(PurchasePrice)}: {PurchasePrice} {nameof(MarketPrice)}: {MarketPrice} {nameof(Amount)}: {Amount} {nameof(Profit)}: {Profit} {nameof(TotalProfit)}: {TotalProfit}";
		}
	}
}
