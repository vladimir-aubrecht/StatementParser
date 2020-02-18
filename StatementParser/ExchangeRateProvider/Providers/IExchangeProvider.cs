using System;
using System.Threading.Tasks;
using ExchangeRateProvider.Models;

namespace ExchangeRateProvider.Providers
{
	public interface IExchangeProvider
	{
		Task<ICurrencyList> FetchCurrencyListByDateAsync(DateTime date);
	}
}
