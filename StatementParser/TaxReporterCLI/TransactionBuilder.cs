using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateProvider.Providers;
using StatementParser.Models;
using TaxReporterCLI.Models;
using ExchangeRateProvider.Providers.Extensions;
using System.Linq;
using ExchangeRateProvider.Models;
using TaxReporterCLI.Models.Views;

namespace TaxReporterCLI
{
    public class TransactionViewBuilder
    {
        private readonly IExchangeProvider exchangeProviderPerYear;
        private readonly IExchangeProvider exchangeProviderPerDay;

        public TransactionViewBuilder(IExchangeProvider exchangeProviderPerYear, IExchangeProvider exchangeProviderPerDay)
        {
            this.exchangeProviderPerYear = exchangeProviderPerYear ?? throw new ArgumentNullException(nameof(exchangeProviderPerYear));
            this.exchangeProviderPerDay = exchangeProviderPerDay ?? throw new ArgumentNullException(nameof(exchangeProviderPerDay));
        }

        public async Task<IList<TransactionView>> BuildAsync(IList<Transaction> transactions)
        {
            var output = new List<TransactionView>();

            var kurzyCurrencyListPerYear = await exchangeProviderPerYear.FetchCurrencyListsForDatesAsync(transactions.Select(i => new DateTime(i.Date.Year, 1, 1)).ToHashSet());
            var cnbCurrencyListPerDay = await exchangeProviderPerDay.FetchCurrencyListsForDatesAsync(transactions.Select(i => i.Date.Date).ToHashSet());

            foreach (var transaction in transactions)
            {
                var currency = transaction.Currency.ToString();
                decimal? exchangeRatePerYear = GetExchangeRatePerYear(kurzyCurrencyListPerYear, transaction, currency);

                var exchangeRatePerDay = cnbCurrencyListPerDay[transaction.Date.Date][currency].Price;

                output.Add(CreateTransactionView(transaction, exchangeRatePerDay, exchangeRatePerYear, Currency.CZK));
            }

            return output;
        }

        private static TransactionView CreateTransactionView(Transaction transaction, decimal? exchangeRatePerDay, decimal? exchangeRatePerYear, Currency exchangedToCurrency)
        {
            var type = transaction.GetType();

            var typeToCreate = new Dictionary<Type, Func<TransactionView>>()
            {
                {typeof(DividendTransaction),
                    () => new DividendTransactionView(transaction as DividendTransaction, exchangeRatePerDay, exchangeRatePerYear, exchangedToCurrency)},
                {typeof(DepositTransaction),
                    () => new DepositTransactionView(transaction as DepositTransaction, exchangeRatePerDay, exchangeRatePerYear, exchangedToCurrency)},
                {typeof(ESPPTransaction),
                    () => new ESPPTransactionView(transaction as ESPPTransaction, exchangeRatePerDay, exchangeRatePerYear, exchangedToCurrency)},
                {typeof(SaleTransaction),
                    () => new SaleTransactionView(transaction as SaleTransaction, exchangeRatePerDay, exchangeRatePerYear, exchangedToCurrency)},
                {typeof(Transaction),
                    () => new TransactionView(transaction, exchangeRatePerDay, exchangeRatePerYear, exchangedToCurrency)}
            };

            if (!typeToCreate.ContainsKey(type))
            {
                return typeToCreate[typeof(Transaction)]();
            }

            return typeToCreate[type]();
        }

        private static decimal? GetExchangeRatePerYear(IDictionary<DateTime, ICurrencyList> kurzyCurrencyListPerYear, Transaction transaction, string currency)
        {
            decimal? exchangeRatePerYear = null;
            var yearDate = new DateTime(transaction.Date.Year, 1, 1);
            if (!kurzyCurrencyListPerYear[yearDate].IsEmpty)
            {
                exchangeRatePerYear = kurzyCurrencyListPerYear[yearDate][currency].Price;
            }

            return exchangeRatePerYear;
        }
    }
}