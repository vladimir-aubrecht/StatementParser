using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Commander.NET;
using Commander.NET.Exceptions;
using ExchangeRateProvider.Providers.Czk;
using StatementParser;
using StatementParser.Models;
using TaxReporterCLI.Models;
using TaxReporterCLI.Models.Views;

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
            var kurzyCzProvider = new KurzyCzProvider();

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

            Console.WriteLine("Downloading exchange rates ...");

            var builder = new TransactionViewBuilder(kurzyCzProvider, cnbProvider);
            var transactionViews = await builder.BuildAsync(transactions);

            var summaryViews = CreateDividendSummaryViews(transactionViews);

            var views = new List<object>(transactionViews);
            views.AddRange(summaryViews);

            Print(option, views);
        }

        private static IList<object> CreateDividendSummaryViews(IList<TransactionView> transactionViews)
        {
            var summaryViews = new List<object>();

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

        private static void Print(Options option, IList<object> transactions)
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
	}
}
