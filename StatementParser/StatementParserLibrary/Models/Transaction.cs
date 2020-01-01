using System;
namespace StatementParserLibrary.Models
{
    public class Transaction
    {
        public Broker Broker { get; }

        public DateTime Date { get; }

        public string Name { get; }

        public decimal Amount { get; }

        public decimal Price { get; }

        public TransactionType Type { get; }

        public Currency Currency { get; }

        public decimal GrossProceeds { get; }

        public Transaction(Broker broker, DateTime date, string name, TransactionType type, decimal amount, decimal price, decimal grossProceeds, Currency currency)
        {
            this.Broker = broker;
            this.Date = date;
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Amount = amount;
            this.Price = price;
            this.Currency = currency;
            this.GrossProceeds = grossProceeds;
        }

        public override string ToString()
        {
            return $"Broker: {Broker} Date: {Date.ToShortDateString()} Name: {Name} Amount: {Amount} Price: {Price} Gross proceeds: {GrossProceeds}";
        }
    }
}
