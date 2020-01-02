using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CommandLine;
using ExchangeRateProvider.Models;
using ExchangeRateProvider.Providers;
using ExchangeRateProvider.Providers.Czk;
using Newtonsoft.Json;
using StatementParser.Models;

namespace TaxReporterCLI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            args = new string[] { "-i", "/Users/vladimiraubrecht/Downloads/Fidelity Deposit.pdf" };
            //args = new string[] { "-i", "/Users/vladimiraubrecht/Downloads/Fidelity ESPP.pdf" };
            args = new string[] { "-i", "/Users/vladimiraubrecht/Downloads/Microsoft Corporation_31Dec2019_222406.xls" };

            var parser = new Parser(with => with.EnableDashDash = true);

            var result = parser.ParseArguments<Options>(args)
                            .WithParsed(async options => await RunAsync(options));

        }

        private static async Task RunAsync(Options option)
        {
            var cnbProvider = new CzechNationalBankProvider();
            var kurzyCzProvider = new KurzyCzProvider();



            var parser = new StatementParser.StatementParser();
            var result = parser.Parse(option.StatementFilePath);

            var kurzyPerYear = await FetchExchangeRatesForEveryYearAsync(kurzyCzProvider, result.Transactions);

            foreach (var transaction in result.Transactions)
            {
                var cnbCurrencyList = await cnbProvider.FetchCurrencyListByDateAsync(transaction.Date);
                var cnbPrice = cnbCurrencyList[transaction.Currency.ToString()].Price;

                // TODO: Refactor this, it's ugly like hell.
                if (transaction is DepositTransaction)
                {
                    var castedTransaction = transaction as DepositTransaction;

                    if (!kurzyPerYear[transaction.Date.Year].IsEmpty)
                    {
                        var kurzyPrice = kurzyPerYear[transaction.Date.Year][transaction.Currency.ToString()].Price;
                        Console.WriteLine($"{transaction} Price in CZK (CNB): {castedTransaction.Price * cnbPrice} Price in CZK (year average): {castedTransaction.Price * kurzyPrice}");
                    }
                    else
                    {
                        Console.WriteLine($"{transaction} Price in CZK (CNB): {castedTransaction.Price * cnbPrice} Price in CZK (year average): N/A");
                    }
                }
                else if (transaction is DividendTransaction)
                {
                    var castedTransaction = transaction as DividendTransaction;

                    if (!kurzyPerYear[transaction.Date.Year].IsEmpty)
                    {
                        var kurzyPrice = kurzyPerYear[transaction.Date.Year][transaction.Currency.ToString()].Price;
                        Console.WriteLine($"{transaction} Income in CZK (CNB): {castedTransaction.Income * cnbPrice} Income in CZK (year average): {castedTransaction.Income * kurzyPrice} Tax in CZK (CNB): {castedTransaction.Tax * cnbPrice} Tax in CZK (year average): {castedTransaction.Tax * kurzyPrice}");
                    }
                    else
                    {
                        Console.WriteLine($"{transaction} Income in CZK (CNB): {castedTransaction.Income * cnbPrice} Income in CZK (year average): N/A Tax in CZK (CNB): {castedTransaction.Tax * cnbPrice} Tax in CZK (year average): N/A");
                    }
                }
            }
        }

        private static async Task<IDictionary<int, CurrencyList>> FetchExchangeRatesForEveryYearAsync(IExchangeProvider provider, IList<Transaction> transactions)
        {
            var years = transactions.Select(i => i.Date.Year).ToHashSet();

            var output = new Dictionary<int, CurrencyList>();

            foreach (var year in years)
            {
                output.Add(year, await provider.FetchCurrencyListByDateAsync(new DateTime(year, 1, 1)));
            }

            return output;
        }
    }
}
