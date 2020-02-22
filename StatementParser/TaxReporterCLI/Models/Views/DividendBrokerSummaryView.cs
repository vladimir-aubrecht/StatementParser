using System;
using System.Collections.Generic;
using System.Linq;
using StatementParser.Models;

namespace TaxReporterCLI.Models.Views
{
	public class DividendBrokerSummaryView
	{
		public Broker Broker { get; }
		public Currency Currency { get; }
		public decimal TotalIncome { get; }
		public decimal TotalTax { get; }

		public decimal ExchangedPerDayTotalIncome { get; }
		public decimal ExchangedPerYearTotalIncome { get; }
		public decimal ExchangedPerDayTotalTax { get; }
		public decimal ExchangedPerYearTotalTax { get; }

		public DividendBrokerSummaryView(IList<DividendTransactionView> transactions, Broker broker, Currency currency)
		{
			this.Broker = broker;
			this.Currency = currency;

			var brokerTransactions = transactions.Where(i => i.Transaction.Broker == broker && i.Transaction.Currency == currency);

			foreach (var transaction in brokerTransactions)
			{
				var dividendTransaction = (transaction.Transaction as DividendTransaction);

				TotalIncome +=  dividendTransaction.Income;
				TotalTax += dividendTransaction.Tax;

				if (transaction.ExchangedPerDayIncome.HasValue)
				{
					ExchangedPerDayTotalIncome += transaction.ExchangedPerDayIncome.Value;
				}

				if (transaction.ExchangedPerYearIncome.HasValue)
				{
					ExchangedPerYearTotalIncome += transaction.ExchangedPerYearIncome.Value;
				}

				if (transaction.ExchangedPerDayTax.HasValue)
				{
					ExchangedPerDayTotalTax += transaction.ExchangedPerDayTax.Value;
				}

				if (transaction.ExchangedPerYearTax.HasValue)
				{
					ExchangedPerYearTotalTax += transaction.ExchangedPerYearTax.Value;
				}
			}
		}

		public override string ToString()
		{
			return $"{nameof(Broker)}: {Broker} {nameof(Currency)}: {Currency} {nameof(TotalIncome)}: {TotalIncome} {nameof(TotalTax)}: {TotalTax} {nameof(ExchangedPerDayTotalIncome)}: {ExchangedPerDayTotalIncome} {nameof(ExchangedPerYearTotalIncome)}: {ExchangedPerYearTotalIncome} {nameof(ExchangedPerDayTotalTax)}: {ExchangedPerDayTotalTax} {nameof(ExchangedPerYearTotalTax)}: {ExchangedPerYearTotalTax}";
		}
	}
}