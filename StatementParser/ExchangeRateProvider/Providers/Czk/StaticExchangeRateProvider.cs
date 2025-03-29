using ExchangeRateProvider.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateProvider.Providers.Czk
{
    public class StaticExchangeRateProvider : IExchangeProvider
    {
        private readonly IDictionary<string, decimal> currencyToPriceDictionary;

        public StaticExchangeRateProvider(IDictionary<string, decimal> currencyToPriceDictionary)
        {
            this.currencyToPriceDictionary = currencyToPriceDictionary;
        }

        public Task<ICurrencyList> FetchCurrencyListByDateAsync(DateTime date)
        {
            var currencyList = new List<CurrencyDescriptor>();

            foreach (var currencyToPrice in currencyToPriceDictionary)
            {
                currencyList.Add(new CurrencyDescriptor(currencyToPrice.Key, "N/A", currencyToPrice.Value, "N/A"));
            }

            return Task.FromResult(new CurrencyList(currencyList) as ICurrencyList);
        }
    }
}
