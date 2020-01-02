using System;
namespace StatementParser.Models
{
    public class DiscountBuyTransaction : Transaction
    {
        public decimal PurchasePrice { get; }
        public decimal MarketPrice { get; }
        public decimal Amount { get; }

        public DiscountBuyTransaction(Broker broker, DateTime date, string name, Currency currency, decimal purchasePrice, decimal marketPrice, decimal amount) : base(broker, date, name, currency)
        {
            this.PurchasePrice = purchasePrice;
            this.MarketPrice = marketPrice;
            this.Amount = amount;
        }

        public override string ToString()
        {
            return $"{base.ToString()} Purchase Price: {PurchasePrice} Market Price: {MarketPrice} Amount: {Amount}";
        }
    }
}
