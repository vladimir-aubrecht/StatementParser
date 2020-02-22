using System;
using System.Collections.Generic;
using System.Linq;
using StatementParser.Models;

namespace TaxReporterCLI.Models.Views
{
	public class DividendBrokerSummaryView : TransactionView
	{
		public DividendBrokerSummaryView(IList<Transaction> transactions, Broker broker, Currency currency, decimal? exchangeRatePerDay, decimal? exchangeRatePerYear) : base(AggregateTransactions(transactions, broker, currency), exchangeRatePerDay, exchangeRatePerYear)
		{

		}

		private static Transaction AggregateTransactions(IList<Transaction> transactions, Broker broker, Currency currency)
		{
			var brokerTransactions = transactions.Where(i => i.Broker == broker && i.Currency == currency).OfType<DividendTransaction>();

			decimal income = 0;
			decimal tax = 0;
			foreach (var transaction in brokerTransactions)
			{
				income += transaction.Income;
				tax += transaction.Tax;
			}

			//return new DividendTransaction(broker, null,  );
			return null;
		}

		public override string ToString()
		{
			return $"{Transaction} {nameof(ExchangeRatePerDay)}: {ExchangeRatePerDay} {nameof(ExchangeRatePerYear)}: {ExchangeRatePerYear}";
		}
	}
}