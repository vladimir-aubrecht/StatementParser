using System;
using StatementParser.Attributes;

namespace StatementParser.Models
{
	public class ESPPTransaction : Transaction
	{
		[Description("Purchase Price")]
		public decimal PurchasePrice { get; }

		[Description("Market Price")]
		public decimal MarketPrice { get; }

		public decimal Amount { get; }

		// Per-share price difference. May not equal TotalProfit / Amount due to
		// broker-side rounding when GainFromPurchase is provided.
		public decimal Profit { get; }

		[Description("Total Profit")]
		public decimal TotalProfit { get; }

		public ESPPTransaction(ESPPTransaction eSPPTransaction) : this(eSPPTransaction.Broker, eSPPTransaction.Date, eSPPTransaction.Name, eSPPTransaction.Currency, eSPPTransaction.PurchasePrice, eSPPTransaction.MarketPrice, eSPPTransaction.Amount, eSPPTransaction.TotalProfit)
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

		public ESPPTransaction(Broker broker, DateTime date, string name, Currency currency, decimal purchasePrice, decimal marketPrice, decimal amount, decimal gainFromPurchase) : base(broker, date, name, currency)
		{
			this.PurchasePrice = purchasePrice;
			this.MarketPrice = marketPrice;
			this.Amount = amount;
			this.Profit = marketPrice - purchasePrice;
			this.TotalProfit = gainFromPurchase;
		}

		public override string ToString()
		{
			return $"{base.ToString()} {nameof(PurchasePrice)}: {PurchasePrice} {nameof(MarketPrice)}: {MarketPrice} {nameof(Amount)}: {Amount} {nameof(Profit)}: {Profit} {nameof(TotalProfit)}: {TotalProfit}";
		}
	}
}
