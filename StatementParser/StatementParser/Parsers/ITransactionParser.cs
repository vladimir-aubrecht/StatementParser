using System.Collections.Generic;
using StatementParser.Models;

namespace StatementParser.Parsers
{
    public interface ITransactionParser
    {
        bool CanParse(string statementFilePath);
        IList<Transaction> Parse(string statementFilePath);
    }
}