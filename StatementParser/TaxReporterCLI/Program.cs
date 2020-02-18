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

			var enrichedTransactions = await CreateEnrichedTransactions(transactions, kurzyCzProvider, cnbProvider);

			Print(option, enrichedTransactions);
		}

		private static void Print(Options option, IList<EnrichedTransaction> transactions)
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

		private static async Task<IList<EnrichedTransaction>> CreateEnrichedTransactions(IList<Transaction> transactions, IExchangeProvider exchangeProviderPerYear, IExchangeProvider exchangeProviderPerDay)
		{
			var kurzyPerYear = await FetchExchangeRatesForEveryYearAsync(exchangeProviderPerYear, transactions);

			var enrichedTransactions = new List<EnrichedTransaction>();
			foreach (var transaction in transactions)
			{
				var cnbCurrencyList = await exchangeProviderPerDay.FetchCurrencyListByDateAsync(transaction.Date);

				var currency = transaction.Currency.ToString();
				decimal exchangeRatePerYear = 0;

				if (!kurzyPerYear[transaction.Date.Year].IsEmpty)
				{
					exchangeRatePerYear = kurzyPerYear[transaction.Date.Year][currency].Price;
				}

				var exchangeRatePerDay = cnbCurrencyList[currency].Price;

				enrichedTransactions.Add(new EnrichedTransaction(transaction, exchangeRatePerDay, exchangeRatePerYear));
			}

			return enrichedTransactions;
		}

		private static async Task<IDictionary<int, ICurrencyList>> FetchExchangeRatesForEveryYearAsync(IExchangeProvider provider, IList<Transaction> transactions)
		{
			var years = transactions.Select(i => i.Date.Year).ToHashSet();

			var output = new Dictionary<int, ICurrencyList>();

			foreach (var year in years)
			{
				output.Add(year, await provider.FetchCurrencyListByDateAsync(new DateTime(year, 1, 1)));
			}

			return output;
		}
	}
}
