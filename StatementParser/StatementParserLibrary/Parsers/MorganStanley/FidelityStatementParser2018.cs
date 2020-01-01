using System;
using StatementParserLibrary.Models;

namespace StatementParserLibrary.Parsers.MorganStanley
{
    public class FidelityStatementParser2018 : IStatementParser
    {
        public bool CanParse(string statementFilePath)
        {
            return false;
        }

        public Statement Parse(string statementFilePath)
        {
            throw new NotImplementedException();
        }
    }
}
