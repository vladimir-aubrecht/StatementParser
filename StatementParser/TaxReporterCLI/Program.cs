using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Commander.NET;
using Commander.NET.Exceptions;
using ExchangeRateProvider.Providers;
using ExchangeRateProvider.Providers.Czk;
using Newtonsoft.Json;
using StatementParser;
using StatementParser.Models;
using TaxReporterCLI.Models.Views;

namespace TaxReporterCLI
{
	public static class Program
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
				var sanitizedPath = path.TrimEnd('/', '\\');

				if (Directory.Exists(sanitizedPath))
				{
					var directoryFiles = Directory.GetFiles(sanitizedPath, "*.*", SearchOption.AllDirectories);
					output.AddRange(directoryFiles);
				}
				else if (File.Exists(sanitizedPath))
				{
					output.Add(sanitizedPath);
				}
			}

			return output;
		}

		private static async Task RunAsync(Options option)
        {
            var cnbProvider = new CzechNationalBankProvider();
            IExchangeProvider yearlyExchangeRateProvider = new KurzyCzProvider();

            if (option.OverrideYearlyExchangeRate != null)
			{
				var overridenExchangeRates = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(option.OverrideYearlyExchangeRate);
                yearlyExchangeRateProvider = new StaticExchangeRateProvider(overridenExchangeRates);
			}

            var parser = new TransactionParser();
            var transactions = new List<Transaction>();
            var filePaths = ResolveFilePaths(option.StatementFilePaths);

            if (filePaths.Count == 0)
            {
                Console.WriteLine("No valid path to scan found. Check that file or directory exist.");
                return;
            }

            foreach (var file in filePaths)
            {
                Console.WriteLine($"Processing file: {file}");
                var result = await parser.ParseAsync(file);

                if (result != null)
                {
                    transactions.AddRange(result);
                }
            }

            transactions = FilterTransactionsByYear(transactions);

            if (transactions.Count == 0)
            {
                Console.WriteLine("No transactions to process.");
                return;
            }

            Console.WriteLine("Downloading exchange rates ...");

            var builder = new TransactionViewBuilder(yearlyExchangeRateProvider, cnbProvider);
            var transactionViews = await builder.BuildAsync(transactions);

            var summaryViews = CreateDividendSummaryViews(transactionViews);

            var views = new List<IView>(transactionViews);
            views.AddRange(summaryViews);

            Print(option, views);
        }

        private static List<Transaction> FilterTransactionsByYear(List<Transaction> transactions)
        {
            var years = transactions.Select(t => t.Date.Year).Distinct().OrderBy(y => y).ToList();

            if (years.Count <= 1)
            {
                return transactions;
            }

            Console.WriteLine();
            Console.WriteLine($"Warning: Transactions span multiple years: {string.Join(", ", years)}");
            Console.WriteLine("Tax reports are typically filed per year. Please choose:");
            Console.WriteLine();

            for (int i = 0; i < years.Count; i++)
            {
                var count = transactions.Count(t => t.Date.Year == years[i]);
                Console.WriteLine($"  {i + 1}) {years[i]} only ({count} transactions)");
            }
            Console.WriteLine($"  {years.Count + 1}) All years combined");
            Console.WriteLine();

            while (true)
            {
                Console.Write($"Enter choice (1-{years.Count + 1}): ");
                var input = Console.ReadLine();

                if (int.TryParse(input, out var choice) && choice >= 1 && choice <= years.Count + 1)
                {
                    if (choice == years.Count + 1)
                    {
                        Console.WriteLine("Proceeding with all transactions.");
                        return transactions;
                    }

                    var selectedYear = years[choice - 1];
                    var filtered = transactions.Where(t => t.Date.Year == selectedYear).ToList();
                    Console.WriteLine($"Proceeding with {filtered.Count} transactions from {selectedYear}.");
                    return filtered;
                }

                Console.WriteLine("Invalid choice. Please try again.");
            }
        }

        private static IList<IView> CreateDividendSummaryViews(IList<TransactionView> transactionViews)
        {
            var summaryViews = new List<IView>();

            var usedBrokers = transactionViews.Select(i => i.Transaction.Broker).Distinct();
            var usedCurrencies = transactionViews.Select(i => i.Transaction.Currency).Distinct();

			foreach (var currency in usedCurrencies)
			{
				foreach (var broker in usedBrokers)
				{
					var brokerSummaryView = new DividendBrokerSummaryView(transactionViews.OfType<DividendTransactionView>().ToList(), broker, currency);

					if (brokerSummaryView.TotalIncome > 0)
					{
						summaryViews.Add(brokerSummaryView);
					}
				}

				var currencySummaryView = new DividendCurrencySummaryView(transactionViews.OfType<DividendTransactionView>().ToList(), currency);

				if (currencySummaryView.TotalIncome > 0)
				{
					summaryViews.Add(currencySummaryView);
				}
            }

            return summaryViews;
        }

        private static void Print(Options option, IList<IView> views)
		{
			var printer = new Output();
			if (option.ShouldPrintAsJson)
			{
				printer.PrintAsJson(views);
			}
			else if (option.ExcelSheetPath != null)
			{
				printer.SaveAsExcelSheet(option.ExcelSheetPath, views);
			}
			else
			{
				printer.PrintAsPlainText(views);
			}
		}
	}
}
