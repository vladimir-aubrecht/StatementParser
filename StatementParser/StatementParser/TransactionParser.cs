using System;
using System.Collections.Generic;
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
            parsers.Add(new FidelityStatementParser());
            parsers.Add(new FxChoiceStatementParser());
        }

        public IList<Transaction> Parse(string statementFilePath)
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
