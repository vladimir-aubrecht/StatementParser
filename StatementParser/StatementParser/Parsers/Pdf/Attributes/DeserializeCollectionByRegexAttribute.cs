using System;
using System.Text.RegularExpressions;

namespace StatementParser.Parsers.Pdf.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DeserializeCollectionByRegexAttribute : Attribute
    {
        public DeserializeCollectionByRegexAttribute(string collectionRegexPattern, string bodyRegexPattern)
        {
            if (string.IsNullOrWhiteSpace(collectionRegexPattern))
            {
                throw new ArgumentException("message", nameof(collectionRegexPattern));
            }

            if (string.IsNullOrWhiteSpace(bodyRegexPattern))
            {
                throw new ArgumentException("message", nameof(bodyRegexPattern));
            }

            this.CollectionRegex = new Regex(collectionRegexPattern, RegexOptions.Compiled);
            this.BodyRegex = new Regex(bodyRegexPattern, RegexOptions.Compiled);
        }

        public Regex CollectionRegex { get; }
        public Regex BodyRegex { get; }
    }
}
