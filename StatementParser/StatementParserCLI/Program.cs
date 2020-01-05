using System;
using System.Collections.Generic;
using System.IO;
using Commander.NET;
using Commander.NET.Exceptions;
using Newtonsoft.Json;
using StatementParser;
using StatementParser.Models;

namespace StatementParserCLI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //args = new string[] { "-json", "/Users/vladimiraubrecht/Downloads/Microsoft Corporation_31Dec2019_222406.xls", "/Users/vladimiraubrecht/Downloads/Fidelity Deposit.pdf", "/Users/vladimiraubrecht/Downloads/Fidelity ESPP.pdf" };
            //args = new string[] { "/Users/vladimiraubrecht/Downloads/Fidelity Deposit.pdf" };
            //args = new string[] { "/Users/vladimiraubrecht/Downloads/Fidelity ESPP.pdf" };
            //args = new string[] { "/Users/vladimiraubrecht/Downloads/Microsoft Corporation_31Dec2019_222406.xls" };
            //args = new string[] { "/Users/vladimiraubrecht/Downloads/Microsoft Corporation_31Dec2019_222406.xls", "/Users/vladimiraubrecht/Downloads/Fidelity Deposit.pdf" };
            //args = new string[] { "/Users/vladimiraubrecht/Documents/Taxes/2019/Statements/FXChoice/January 2019.pdf" };
            //args = new string[] { "/Users/vladimiraubrecht/Documents/Taxes/2019/Statements" };

            var parser = new CommanderParser<Options>();

            try
            {
                var options = parser.Parse(args);
                Run(options);
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

        private static void Run(Options option)
        {
            var parser = new TransactionParser();

            var transactions = new List<Transaction>();
            var filePaths = ResolveFilePaths(option.StatementFilePaths);
            foreach (var file in filePaths)
            {
                Console.WriteLine($"Processing file: {file}");
                var result = parser.Parse(file);

                if (result != null)
                {
                    transactions.AddRange(result);
                }
            }

            if (option.ShouldPrintAsJson)
            {
                PrintAsJson(transactions);
            }
            else
            {
                PrintAsPlainText(transactions);
            }

        }

        private static void PrintAsJson(IList<Transaction> transactions)
        {
            Console.WriteLine(JsonConvert.SerializeObject(transactions));
        }

        private static void PrintAsPlainText(IList<Transaction> transactions)
        {
            Console.WriteLine(String.Join("\r\n", transactions));
        }
    }
}
