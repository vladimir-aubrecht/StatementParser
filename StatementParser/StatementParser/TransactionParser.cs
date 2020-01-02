using System;
using System.Collections.Generic;
using StatementParser.Models;
using StatementParser.Parsers;
using StatementParser.Parsers.Fidelity;
using StatementParser.Parsers.MorganStanley;

namespace StatementParser
{
    public class TransactionParser
    {
        private IList<ITransactionParser> parsers = new List<ITransactionParser>();

        public TransactionParser()
        {
            parsers.Add(new MorganStanleyStatementParser2018());
            parsers.Add(new FidelityStatementParser2018());
        }

        public IList<Transaction> Parse(string statementFilePath)
        {
            foreach (var parser in parsers)
            {
                if (parser.CanParse(statementFilePath))
                {
                    return parser.Parse(statementFilePath);
                }
            }

            return null;
        }
    }
}
