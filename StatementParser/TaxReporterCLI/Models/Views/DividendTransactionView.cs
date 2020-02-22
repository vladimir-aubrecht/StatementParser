using StatementParser.Models;

namespace TaxReporterCLI.Models.Views
{
	public class DividendTransactionView : TransactionView
	{
        public decimal? ExchangedPerDayIncome { get; }
        public decimal? ExchangedPerYearIncome { get; }
        public decimal? ExchangedPerDayTax { get; }
        public decimal? ExchangedPerYearTax { get; }

		public DividendTransactionView(DividendTransaction transaction, decimal? exchangeRatePerDay, decimal? exchangeRatePerYear) : base(transaction, exchangeRatePerDay, exchangeRatePerYear)
		{
            ExchangedPerDayIncome = transaction.Income * exchangeRatePerDay;
            ExchangedPerYearIncome = transaction.Income * exchangeRatePerYear;

            ExchangedPerDayTax = transaction.Tax * exchangeRatePerDay;
            ExchangedPerYearTax = transaction.Tax * exchangeRatePerYear;
		}

        public override string ToString()
        {
            return base.ToString() + $" {nameof(ExchangedPerDayIncome)}: {ExchangedPerDayIncome} {nameof(ExchangedPerYearIncome)}: {ExchangedPerYearIncome} {nameof(ExchangedPerDayTax)}: {ExchangedPerDayTax} {nameof(ExchangedPerYearTax)}: {ExchangedPerYearTax}";
        }
	}
}