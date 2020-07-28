using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using StatementParser.Models;
using StatementParser.Parsers;
using StatementParser.Parsers.Brokers.Degiro;
using StatementParser.Parsers.Brokers.Fidelity;
using StatementParser.Parsers.Brokers.FxChoice;
using StatementParser.Parsers.Brokers.Lynx;
using StatementParser.Parsers.Brokers.MorganStanley;
using StatementParser.Parsers.Brokers.Revolut;

namespace StatementParser
{
    public class TransactionParser
    {
        private readonly IList<ITransactionParser> parsers = new List<ITransactionParser>();

        public TransactionParser(ILoggerFactory loggerFactory)
        {
            parsers.Add(new MorganStanleyStatementXlsParser(loggerFactory.CreateLogger<MorganStanleyStatementXlsParser>()));
            parsers.Add(new MorganStanleyStatementPdfParser(loggerFactory.CreateLogger<MorganStanleyStatementPdfParser>()));
            parsers.Add(new FidelityStatementParser(loggerFactory.CreateLogger<FidelityStatementParser>()));
            parsers.Add(new FxChoiceStatementParser(loggerFactory.CreateLogger<FxChoiceStatementParser>()));
            parsers.Add(new LynxCsvParser(loggerFactory.CreateLogger<LynxCsvParser>()));
            parsers.Add(new DegiroParser(loggerFactory.CreateLogger<DegiroParser>()));
            parsers.Add(new RevolutStatementParser(loggerFactory.CreateLogger<RevolutStatementParser>()));
        }

        public Task<IList<Transaction>> ParseAsync(string statementFilePath)
        {
            return Task.Run(() => ParseFiles(statementFilePath));
        }

        private IList<Transaction> ParseFiles(string statementFilePath)
        {
            foreach (var parser in parsers)
            {
                var result = parser.Parse(statementFilePath);

                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}
