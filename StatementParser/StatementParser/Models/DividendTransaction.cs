using System;
namespace StatementParser.Models
{
    public class DividendTransaction : Transaction
    {
        public decimal Income { get; }

        public decimal Tax { get; }

        public DividendTransaction(Broker broker, DateTime date, string name, decimal income, decimal tax, Currency currency) : base(broker, date, name, currency)
        {
            this.Income = income;
            this.Tax = tax;
        }

        public override string ToString()
        {
            return $"{base.ToString()} Income: {Income} Tax: {Tax}";
        }
    }
}
