using System;
namespace StatementParser.Models
{
    public class SaleTransaction : Transaction
    {
        public SaleTransaction(Broker broker, DateTime date, string name, Currency currency, int laverage, decimal amount, decimal purchasePrice, decimal salePrice, decimal commission, decimal taxes, decimal swap, decimal profit) : base(broker, date, name, currency)
        {
            this.Laverage = laverage;
            this.Amount = amount;
            this.PurchasePrice = purchasePrice;
            this.SalePrice = salePrice;
            this.Commission = commission;
            this.Taxes = taxes;
            this.Swap = swap;
            this.Profit = profit;
        }

        public int Laverage { get; }
        public decimal Amount { get; }
        public decimal PurchasePrice { get; }
        public decimal SalePrice { get; }
        public decimal Commission { get; }
        public decimal Taxes { get; }
        public decimal Swap { get; }
        public decimal Profit { get; }

        public override string ToString()
        {
            return $"{base.ToString()} {nameof(Laverage)} {Laverage} {nameof(Amount)}: {Amount} {nameof(PurchasePrice)} {PurchasePrice} {nameof(SalePrice)} {SalePrice} {nameof(Commission)} {Commission} {nameof(Taxes)} {Taxes} {nameof(Swap)} {Swap} {nameof(Profit)} {Profit}";
        }
    }
}
