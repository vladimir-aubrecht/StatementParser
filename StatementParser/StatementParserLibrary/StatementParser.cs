using System;
using System.Collections.Generic;
using StatementParserLibrary.Models;
using StatementParserLibrary.Parsers;
using StatementParserLibrary.Parsers.MorganStanley;

namespace StatementParserLibrary
{
    public class StatementParser
    {
        private IList<IStatementParser> parsers = new List<IStatementParser>();

        public StatementParser()
        {
            parsers.Add(new MorganStanleyStatementParser2018());
            parsers.Add(new FidelityStatementParser2018());
        }

        public Statement Parse(string statementFilePath)
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
