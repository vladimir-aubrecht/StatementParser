using System.Collections.Generic;
using StatementParser.Models;

namespace StatementParser.Parsers
{
    public interface ITransactionParser
    {
        IList<Transaction> Parse(string statementFilePath);
    }
}