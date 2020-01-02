using System;
namespace StatementParser.Models
{
    public class DepositTransaction : Transaction
    {
        public decimal Amount { get; }

        public decimal Price { get; }

        public DepositTransaction(Broker broker, DateTime date, string name, decimal amount, decimal price, Currency currency) : base(broker, date, name, currency)
        {
            this.Amount = amount;
            this.Price = price;
        }

        public override string ToString()
        {
            return $"{base.ToString()} Amount: {Amount} Price: {Price}";
        }
    }
}
