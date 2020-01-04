using System;
namespace StatementParser.Parsers.Pdf
{
    internal class PdfTableRow
    {
        public PdfTableRow(string tableRowContent)
        {
            this.Value = tableRowContent ?? throw new ArgumentNullException(nameof(tableRowContent));
        }

        public string Value { get; }
    }
}
