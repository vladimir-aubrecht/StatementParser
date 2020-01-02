using StatementParser.Models;

namespace StatementParser.Parsers
{
    public interface IStatementParser
    {
        bool CanParse(string statementFilePath);
        Statement Parse(string statementFilePath);
    }
}