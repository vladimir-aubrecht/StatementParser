using System;
using System.Text.RegularExpressions;

namespace StatementParser.Parsers.Pdf
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    internal class DeserializeByRegexAttribute : Attribute
    {
        public Regex DeserizalizationRegex { get; }
        public Regex PageBodyRegex { get; }
        public Regex RowSplitRegex { get; }

        public DeserializeByRegexAttribute(string deserializationRegexPattern, string pageBodyRegexPattern, string rowSplitRegexPattern)
        {
            if (string.IsNullOrWhiteSpace(deserializationRegexPattern))
            {
                throw new ArgumentException("message", nameof(deserializationRegexPattern));
            }

            if (string.IsNullOrWhiteSpace(pageBodyRegexPattern))
            {
                throw new ArgumentException("message", nameof(pageBodyRegexPattern));
            }

            this.DeserizalizationRegex = new Regex(deserializationRegexPattern, RegexOptions.Compiled);
            this.PageBodyRegex = new Regex(pageBodyRegexPattern, RegexOptions.Compiled);
            this.RowSplitRegex = new Regex(rowSplitRegexPattern, RegexOptions.Compiled);
        }
    }
}
