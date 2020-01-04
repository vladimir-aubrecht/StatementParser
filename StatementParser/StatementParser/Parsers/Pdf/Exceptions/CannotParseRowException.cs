using System;
namespace StatementParser.Parsers.Pdf.Exceptions
{
    public class CannotParseRowException : PdfException
    {
        public CannotParseRowException(string content, Type modelType) : base($"Couldn't deserialize content into class {modelType.Name}. Content: {content}")
        {
        }
    }
}
