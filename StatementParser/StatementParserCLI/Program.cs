using System;
using System.Collections.Generic;
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
            args = new string[] { "/Users/vladimiraubrecht/Downloads/Fidelity Deposit.pdf" };
            //args = new string[] { "/Users/vladimiraubrecht/Downloads/Fidelity ESPP.pdf" };
            //args = new string[] { "/Users/vladimiraubrecht/Downloads/Microsoft Corporation_31Dec2019_222406.xls" };
            //args = new string[] { "/Users/vladimiraubrecht/Downloads/Microsoft Corporation_31Dec2019_222406.xls", "/Users/vladimiraubrecht/Downloads/Fidelity Deposit.pdf" };

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

        private static void Run(Options option)
        {
            var parser = new TransactionParser();

            var transactions = new List<Transaction>();
            foreach (var file in option.StatementFilePaths)
            {
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
