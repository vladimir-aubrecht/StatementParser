using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NPOI.HSSF.Record.Aggregates;
using StatementParser.Models;
using StatementParser.Parsers;
using StatementParser.Parsers.Brokers.Fidelity;
using StatementParser.Parsers.Brokers.FxChoice;
using StatementParser.Parsers.Brokers.MorganStanley;

namespace StatementParser
{
    public class TransactionParser
    {
        private IList<ITransactionParser> parsers = new List<ITransactionParser>();

        public TransactionParser()
        {
            parsers.Add(new MorganStanleyStatementParser());
            parsers.Add(new MorganStanleyStatementPdfParser());
            parsers.Add(new FidelityStatementParser());
            parsers.Add(new FxChoiceStatementParser());
        }

        public Task<IList<Transaction>> ParseAsync(string statementFilePath)
        {
            return RunWithGlobalSettings(() => {
                return ParseFiles(statementFilePath);
            });
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

        private Task<IList<Transaction>> RunWithGlobalSettings(Func<IList<Transaction>> action)
        {
            return Task.Run(() => {
                var culture = new System.Globalization.CultureInfo("en-US");
                Thread.CurrentThread.CurrentCulture = culture;

                return action();
            });
        }
    }
}
