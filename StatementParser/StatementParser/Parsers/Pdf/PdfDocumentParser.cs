using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using StatementParser.Parsers.Pdf.Attributes;
using StatementParser.Parsers.Pdf.Extensions;

namespace StatementParser.Parsers.Pdf
{
	internal class PdfDocumentParser<TDocumentDescriptor> : IPdfDocumentParser<TDocumentDescriptor> where TDocumentDescriptor : new()
	{
		private readonly IPdfRegexDeserializator pdfInstanceCreator;
		private readonly Dictionary<PropertyInfo, DeserializeCollectionByRegexAttribute> deserializeCollectionByRegexAttributes;

		public PdfDocumentParser(IPdfRegexDeserializator pdfInstanceCreator = null)
		{
			this.pdfInstanceCreator = pdfInstanceCreator ?? new PdfTypeByRegexDeserializator();

			deserializeCollectionByRegexAttributes = CollectCollectionByRegexAttributes();
		}

		public TDocumentDescriptor Parse(IPdfSource pdfSource)
		{
			_ = pdfSource ?? throw new ArgumentNullException(nameof(pdfSource));

			var output = new TDocumentDescriptor();

			foreach (var property in typeof(TDocumentDescriptor).GetProperties())
			{
				var body = ParseTableContent(pdfSource.GetPagesText(), deserializeCollectionByRegexAttributes[property]?.BodyRegex);

				var value = this.pdfInstanceCreator.Deserialize(
					body,
					property.PropertyType,
					typeof(TDocumentDescriptor),
					deserializeCollectionByRegexAttributes[property]?.CollectionRegex,
					GetDeserializationRegexByType(typeof(TDocumentDescriptor)),
					GetDeserializationRegexByType(property.PropertyType),
					GetDeserializationRegexByType(property.PropertyType.GetCollectionElementType()));

				if (this.pdfInstanceCreator.IsSimpleType(property.PropertyType))
				{
					property.SetValue(output, property.GetValue(value));
				}
				else
				{
					property.SetValue(output, value);
				}
			}

			return output;
		}

		private Dictionary<PropertyInfo, DeserializeCollectionByRegexAttribute> CollectCollectionByRegexAttributes()
		{
			return typeof(TDocumentDescriptor).GetProperties()
				.ToDictionary(k => k, i => i.GetCustomAttribute<DeserializeCollectionByRegexAttribute>(false));
		}

		private Regex GetDeserializationRegexByType(Type type)
		{
			return type?.GetCustomAttribute<DeserializeByRegexAttribute>(true)?.DeserizalizationRegex;
		}

		private string ParseTableContent(IEnumerable<string> content, Regex pageBodyRegex)
		{
			if (pageBodyRegex == null)
			{
				return String.Join("", content);
			}

			var contents = content
				.Where(pageText => pageBodyRegex.Match(pageText).Success)
				.Select(pageText => pageBodyRegex.Match(pageText).Groups[1].Value);

			return String.Join("", contents); //Lets merge tables splitted cross multiple pages
		}
	}
}
