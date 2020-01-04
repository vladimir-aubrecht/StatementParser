using System;
using System.Text.RegularExpressions;

namespace StatementParser.Parsers.Pdf
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    internal class DeserializeByRegexAttribute : Attribute
    {
        public Regex ParsingRegex { get; }

        public DeserializeByRegexAttribute(string regexPattern)
        {
            if (string.IsNullOrWhiteSpace(regexPattern))
            {
                throw new ArgumentException("message", nameof(regexPattern));
            }

            this.ParsingRegex = new Regex(regexPattern, RegexOptions.Compiled);
        }
    }
}
