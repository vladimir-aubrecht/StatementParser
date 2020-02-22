using StatementParser.Models;

namespace TaxReporterCLI.Models.Views
{
	public class SaleTransactionView : TransactionView
	{
        public decimal? ExchangedPerDayProfit { get; }
        public decimal? ExchangedPerYearProfit { get; }
        public decimal? ExchangedPerDaySwap { get; }
        public decimal? ExchangedPerYearSwap { get; }
        public decimal? ExchangedPerDayTaxes { get; }
        public decimal? ExchangedPerYearTaxes { get; }
        public decimal? ExchangedPerDayCommission { get; }
        public decimal? ExchangedPerYearCommission { get; }

		public SaleTransactionView(SaleTransaction transaction, decimal? exchangeRatePerDay, decimal? exchangeRatePerYear) : base(transaction, exchangeRatePerDay, exchangeRatePerYear)
		{
            ExchangedPerDayProfit = transaction.Profit * exchangeRatePerDay;
            ExchangedPerYearProfit = transaction.Profit * exchangeRatePerYear;

            ExchangedPerDaySwap = transaction.Swap * exchangeRatePerDay;
            ExchangedPerYearSwap = transaction.Swap * exchangeRatePerYear;

            ExchangedPerDayTaxes = transaction.Taxes * exchangeRatePerDay;
            ExchangedPerYearTaxes = transaction.Taxes * exchangeRatePerYear;

            ExchangedPerDayCommission = transaction.Commission * exchangeRatePerDay;
            ExchangedPerYearCommission = transaction.Commission * exchangeRatePerYear;
		}

        public override string ToString()
        {
            return base.ToString() 
            + $" {nameof(ExchangedPerDayProfit)}: {ExchangedPerDayProfit} {nameof(ExchangedPerYearProfit)}: {ExchangedPerYearProfit} {nameof(ExchangedPerDaySwap)}: {ExchangedPerDaySwap} {nameof(ExchangedPerYearSwap)}: {ExchangedPerYearSwap}"
            + $" {nameof(ExchangedPerDayTaxes)}: {ExchangedPerDayTaxes} {nameof(ExchangedPerYearTaxes)}: {ExchangedPerYearTaxes} {nameof(ExchangedPerDayCommission)}: {ExchangedPerDayCommission} {nameof(ExchangedPerYearCommission)}: {ExchangedPerYearCommission}";
        }
	}
}