using System;
using System.Collections.Generic;
using System.Reflection;
using StatementParser.Parsers.Pdf.Exceptions;

namespace StatementParser.Parsers.Pdf
{
    internal class PdfTableRow<TRowDescriptor> where TRowDescriptor : new()
    {
        public PdfTableRow(string tableRowContent)
        {
            this.RawValue = tableRowContent ?? throw new ArgumentNullException(nameof(tableRowContent));

            this.Value = CreateRow(ParsePropertiesByAttribute());
        }

        public string RawValue { get; }

        public TRowDescriptor Value { get; }

        private TRowDescriptor CreateRow(IDictionary<string, string> properties)
        {
            var result = new TRowDescriptor();

            foreach (var property in properties)
            {
                var propertyInfo = typeof(TRowDescriptor).GetProperty(property.Key)
                    ?? throw new InvalidOperationException($"Trying to set property with name: {property.Key}, but such property cannot be found.");

                propertyInfo.SetValue(result, property.Value);
            }

            return result;
        }

        private IDictionary<string, string> ParsePropertiesByAttribute()
        {
            var attribute = typeof(TRowDescriptor).GetCustomAttribute<DeserializeByRegexAttribute>(true)
                ?? throw new InvalidOperationException($"Class {nameof(TRowDescriptor)} must use {nameof(DeserializeByRegexAttribute)} attribute.");

            var matchResults = attribute.ParsingRegex.Match(RawValue);

            var output = new Dictionary<string, string>();

            if (matchResults.Success)
            {
                foreach (var groupName in attribute.ParsingRegex.GetGroupNames())
                {
                    if (groupName == "0")
                    {
                        continue;
                    }

                    var value = matchResults.Groups[groupName].Value;
                    output.Add(groupName, value);
                }
            }
            else
            {
                throw new CannotParseRowException(RawValue, typeof(TRowDescriptor));
            }

            return output;
        }
    }
}
