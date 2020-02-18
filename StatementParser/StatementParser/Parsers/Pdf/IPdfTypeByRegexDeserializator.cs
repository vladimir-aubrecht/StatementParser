using System;
using System.Text.RegularExpressions;

namespace StatementParser.Parsers.Pdf
{
	internal interface IPdfRegexDeserializator
	{
		object Deserialize(string content, Type propertyType, Type propertyClassType, Regex collectionRegex, Regex documentDeserializationRegex, Regex propertyDeserializationRegex, Regex elementDeserializationRegex);
		object DeserializeCollection(Type type, string content, Regex deserializationRegex, Regex collectionSplitRegex);
		object DeserializeClass(Type type, string content, Regex deserializationRegex);
		bool IsSimpleType(Type type);
	}
}