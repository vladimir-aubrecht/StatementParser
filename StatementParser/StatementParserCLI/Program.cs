using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Commander.NET;
using Commander.NET.Exceptions;
using Newtonsoft.Json;
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
