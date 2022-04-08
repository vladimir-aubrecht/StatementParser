using System;
using StatementParser.Attributes;

namespace StatementParser.Models
{
	public class SaleTransaction : Transaction
	{
		public SaleTransaction(SaleTransaction saleTransaction) : this(saleTransaction.Broker, saleTransaction.Date, saleTransaction.Name, saleTransaction.Currency, saleTransaction.Leverage, saleTransaction.Amount, saleTransaction.PurchasePrice, saleTransaction.SalePrice, saleTransaction.Commission, saleTransaction.Taxes, saleTransaction.Swap, saleTransaction.Profit)
		{
		}

		public SaleTransaction(Broker broker, DateTime date, string name, Currency currency, int leverage, decimal amount, decimal purchasePrice, decimal salePrice, decimal commission, decimal taxes, decimal swap, decimal profit) : base(broker, date, name, currency)
		{
			this.Leverage = leverage;
			this.Amount = amount;
			this.PurchasePrice = purchasePrice;
			this.SalePrice = salePrice;
			this.Commission = Math.Abs(commission);
			this.Taxes = Math.Abs(taxes);
			this.Swap = swap;
			this.Profit = profit;
		}

		public int Leverage { get; }
		public decimal Amount { get; }

		[Description("Purchase Price")]
		public decimal PurchasePrice { get; }

		[Description("Sale Price")]
		public decimal SalePrice { get; }

		public decimal Commission { get; }

		public decimal Taxes { get; }

		public decimal Swap { get; }

		public decimal Profit { get; }

		public override string ToString()
		{
			return $"{base.ToString()} {nameof(Leverage)} {Leverage} {nameof(Amount)}: {Amount} {nameof(PurchasePrice)} {PurchasePrice} {nameof(SalePrice)} {SalePrice} {nameof(Commission)} {Commission} {nameof(Taxes)} {Taxes} {nameof(Swap)} {Swap} {nameof(Profit)} {Profit}";
		}
	}
}
