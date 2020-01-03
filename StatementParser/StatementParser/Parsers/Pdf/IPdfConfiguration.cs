using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace StatementParser.Parsers.Pdf
{
    internal interface IPdfConfiguration
    {
        IDictionary<PdfTableName, Dictionary<Separators, string>> TableRestrictorsMap { get; }

        IDictionary<PdfTableName, Regex> TableRowSplitRegexes { get; }
    }
}
