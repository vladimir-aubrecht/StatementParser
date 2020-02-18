using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Commander.NET;
using Commander.NET.Exceptions;
using StatementParser;
using StatementParser.Models;

namespace StatementParserCLI
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
