using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateProvider.Models;

namespace ExchangeRateProvider.Providers.Extensions
{
    public static class IExchangeProviderExtensions
    {
        public static async Task<IDictionary<DateTime, ICurrencyList>> FetchCurrencyListsForDatesAsync(this IExchangeProvider exchangeProvider, ISet<DateTime> dates)
        {
            var output = new Dictionary<DateTime, ICurrencyList>();

			foreach (var date in dates)
			{
				output.Add(date, await exchangeProvider.FetchCurrencyListByDateAsync(date).ConfigureAwait(false));
			}

			return output;
        }
    }
}