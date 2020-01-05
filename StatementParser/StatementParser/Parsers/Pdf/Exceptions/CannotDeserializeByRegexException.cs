using System;
using System.Text.RegularExpressions;

namespace StatementParser.Parsers.Pdf.Exceptions
{
    public class CannotDeserializeByRegexException : PdfException
    {
        public CannotDeserializeByRegexException(string content, Regex regex) : base($"Couldn't deserialize content by regex pattern: {regex}. Content: {content}")
        {
        }
    }
}