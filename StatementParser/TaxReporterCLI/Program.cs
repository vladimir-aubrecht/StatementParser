using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Commander.NET;
using Commander.NET.Exceptions;
using ExchangeRateProvider.Models;
using ExchangeRateProvider.Providers;
using ExchangeRateProvider.Providers.Czk;
using StatementParser;
using StatementParser.Models;

namespace TaxReporterCLI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var parser = new CommanderParser<Options>();

            try
            {
                var options = parser.Parse(args);
                await RunAsync(options);
            }
            catch (ParameterMissingException)
            {
                Console.WriteLine(parser.Usage());
            }

        }

        private static IList<string> ResolveFilePaths(string[] paths)
        {
            var output = new List<string>();
            foreach (var path in paths)
            {
                if (Directory.Exists(path))
                {
                    var directoryFiles = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
                    output.AddRange(directoryFiles);
                }
                else if (File.Exists(path))
                {
                    output.Add(path);
                }
            }

            return output;
        }

        private static async Task RunAsync(Options option)
        {
            var cnbProvider = new CzechNationalBankProvider();
            var kurzyCzProvider = new KurzyCzProvider();

            var parser = new TransactionParser();
            var transactions = new List<Transaction>();
            var filePaths = ResolveFilePaths(option.StatementFilePaths);
            foreach (var file in filePaths)
            {
                Console.WriteLine($"Processing file: {file}");
                var result = await parser.ParseAsync(file);

                if (result != null)
                {
                    transactions.AddRange(result);
                }
            }

            var kurzyPerYear = await FetchExchangeRatesForEveryYearAsync(kurzyCzProvider, transactions);

            var transactionsByPerYearExchangeRate = 
                transactions.Select(i => {
                    if (!kurzyPerYear[i.Date.Year].IsEmpty)
                    {
                        var exchangeRatio = kurzyPerYear[i.Date.Year][i.Currency.ToString()].Price;
                        return i.ConvertToCurrency(Currency.CZK, exchangeRatio);
                    }

                    return i;

                    }).ToList();

            var transactionsByPerDayExchangeRateTasks =
                transactions.Select(async i => {
                    var cnbCurrencyList = await cnbProvider.FetchCurrencyListByDateAsync(i.Date);    
                    return i.ConvertToCurrency(Currency.CZK, cnbCurrencyList[i.Currency.ToString()].Price);
                }).ToList();

            await Task.WhenAll(transactionsByPerDayExchangeRateTasks);
            var transactionsByPerDayExchangeRate = transactionsByPerDayExchangeRateTasks.Select(i => i.Result).ToList();

            var pathBackup = option.ExcelSheetPath;

            Console.WriteLine("Transactions calculated with yearly average exchange rate:");
            option.ExcelSheetPath = Path.ChangeExtension(pathBackup, "yearly.xlsx");
            Print(option, transactionsByPerYearExchangeRate);
            
            Console.WriteLine("\r\nTransactions calculated with daily exchange rate:");
            option.ExcelSheetPath = Path.ChangeExtension(pathBackup, "daily.xlsx");
            Print(option, transactionsByPerDayExchangeRate);
        }

        private static void Print(Options option, IList<Transaction> transactions)
        {
            var printer = new Output();
            if (option.ShouldPrintAsJson)
            {
                printer.PrintAsJson(transactions);
            }
            else if (option.ExcelSheetPath != null)
            {
                printer.SaveAsExcelSheet(option.ExcelSheetPath, transactions);
            }
            else
            {
                printer.PrintAsPlainText(transactions);
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
