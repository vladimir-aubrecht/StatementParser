using System;
using System.Text.RegularExpressions;

namespace StatementParser.Parsers.Pdf.Attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	internal class DeserializeByRegexAttribute : Attribute
	{
		public Regex DeserizalizationRegex { get; }

		public DeserializeByRegexAttribute(string deserializationRegexPattern)
		{
			if (string.IsNullOrWhiteSpace(deserializationRegexPattern))
			{
				throw new ArgumentException("message", nameof(deserializationRegexPattern));
			}

			this.DeserizalizationRegex = new Regex(deserializationRegexPattern, RegexOptions.Compiled);
		}
	}
}
