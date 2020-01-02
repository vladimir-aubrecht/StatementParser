using System;
using Commander.NET;
using Commander.NET.Exceptions;
using Newtonsoft.Json;
using StatementParser;

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

            foreach (var file in option.StatementFilePaths)
            {
                var result = parser.Parse(file);

                var output = String.Join("\r\n", result);
                if (option.ShouldPrintAsJson)
                {
                    output = JsonConvert.SerializeObject(result);
                }

                Console.WriteLine(output);
            }
        }
    }
}
