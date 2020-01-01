using System;
using System.Threading.Tasks;
using CommandLine;
using Newtonsoft.Json;

namespace StatementParser
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            args = new string[] { "-i", "/Users/vladimiraubrecht/Downloads/Fidelity Deposit.pdf" };
            args = new string[] { "-i", "/Users/vladimiraubrecht/Downloads/Fidelity ESPP.pdf" };
            args = new string[] { "-i", "/Users/vladimiraubrecht/Downloads/Microsoft Corporation_31Dec2019_222406.xls" };

            var parser = new Parser(with => with.EnableDashDash = true);

            var result = parser.ParseArguments<Options>(args)
                            .WithParsed(async options => await RunAsync(options));

        }

        private static async Task RunAsync(Options option)
        {
            var parser = new StatementParserLibrary.StatementParser();
            var result = parser.Parse(option.StatementFilePath);

            var output = result.ToString();
            if (option.ShouldPrintAsJson)
            {
                output = JsonConvert.SerializeObject(result);
            }

            Console.WriteLine(output);
        }
    }
}
