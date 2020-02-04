using System;
namespace StatementParser.Models
{
    public class DividendTransaction : Transaction
    {
        /// <summary>
        /// Income before taxation
        /// </summary>
        public decimal Income { get; }

        public decimal Tax { get; }

        public DividendTransaction(Broker broker, DateTime date, string name, decimal income, decimal tax, Currency currency) : base(broker, date, name, currency)
        {
            this.Income = income;
            this.Tax = tax;
        }

        public override Transaction ConvertToCurrency(Currency currency, decimal exchangeRate)
        {
            return new DividendTransaction(Broker, Date, Name, Income * exchangeRate, Tax * exchangeRate, currency);
        }

        public override string ToString()
        {
            return $"{base.ToString()} Income: {Income} Tax: {Tax}";
        }
    }
}
