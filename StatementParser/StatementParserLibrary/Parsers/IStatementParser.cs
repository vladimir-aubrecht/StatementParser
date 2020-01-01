using StatementParserLibrary.Models;

namespace StatementParserLibrary.Parsers
{
    public interface IStatementParser
    {
        bool CanParse(string statementFilePath);
        Statement Parse(string statementFilePath);
    }
}