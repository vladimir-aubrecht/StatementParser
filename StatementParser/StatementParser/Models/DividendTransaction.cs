using System;
using System.Globalization;

namespace StatementParser.Models
{
    public class DividendTransaction : Transaction
    {
        /// <summary>
        /// Income before taxation
        /// </summary>
        public decimal Income { get; }

        public decimal Tax { get; }

        public RegionInfo TaxCountry { get; }

        public DividendTransaction(DividendTransaction dividendTransaction) : this(dividendTransaction.Broker, dividendTransaction.Date, dividendTransaction.Name, dividendTransaction.Income, dividendTransaction.Tax, dividendTransaction.TaxCountry, dividendTransaction.Currency)
        {
        }

        public DividendTransaction(Broker broker, DateTime date, string name, decimal income, decimal tax, RegionInfo taxCountry, Currency currency) : base(broker, date, name, currency)
        {
            this.Income = income;
            this.Tax = tax;
            this.TaxCountry = taxCountry;
        }

        public override string ToString()
        {
            return $"{base.ToString()} Income: {Income} Tax: {Tax}";
        }
    }
}
