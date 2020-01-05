using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using StatementParser.Parsers.Pdf.Exceptions;
using StatementParser.Parsers.Pdf.Extensions;

namespace StatementParser.Parsers.Pdf
{
    internal class PdfTypeByRegexDeserializator : IPdfRegexDeserializator
    {
        public object DeserializeCollection(Type type, string content, Regex deserializationRegex, Regex collectionSplitRegex)
        {
            var items = SplitTableContentIntoRows(content, collectionSplitRegex);
            return CreateCollectionInstance(type, type.GetCollectionElementType(), items, deserializationRegex);
        }

        public object DeserializeClass(Type type, string content, Regex deserializationRegex)
        {
            var properties = ParsePropertiesByAttribute(content, deserializationRegex);
            return CreateInstance(type, properties);
        }

        public object Deserialize(string content, Type propertyType, Type propertyClassType, Regex collectionRegex, Regex documentDeserializationRegex, Regex propertyDeserializationRegex, Regex elementDeserializationRegex)
        {
            var isCollection = collectionRegex != null;

            if (isCollection)
            {
                return this.DeserializeCollection(propertyType, content, elementDeserializationRegex, collectionRegex);
            }
            else if (propertyType.IsPrimitive)
            {
                return this.DeserializeClass(propertyClassType, content, documentDeserializationRegex);
            }
            else
            {
                return this.DeserializeClass(propertyType, content, propertyDeserializationRegex);
            }
        }


        private object ConvertToCollectionOfType(IList<object> items, Type propertyType)
        {
            return JsonConvert.DeserializeObject(JsonConvert.SerializeObject(items), propertyType);   // to handle array/list/hashset/...
        }

        private object ConvertValueToPropertyType(Type propertyType, string value)
        {
            return Convert.ChangeType(value, propertyType);
        }

        private object CreateCollectionInstance(Type propertyType, Type itemType, IList<string> items, Regex deserializationRegex)
        {
            var output = new List<object>();
            foreach (var item in items)
            {
                if (itemType != null)
                {
                    try
                    {
                        var instance = this.DeserializeClass(itemType, item, deserializationRegex);

                        output.Add(instance);
                    }
                    catch (CannotDeserializeByRegexException)
                    {
                        // We want to deserialize into rows which satisfy regex
                        continue;
                    }
                }
            }

            return ConvertToCollectionOfType(output, propertyType);
        }

        private object CreateInstance(Type type, IDictionary<string, string> properties)
        {
            var result = Activator.CreateInstance(type);

            foreach (var property in properties)
            {
                var propertyInfo = type.GetProperty(property.Key)
                    ?? throw new InvalidOperationException($"Trying to set property with name: {property.Key}, but such property cannot be found.");

                var convertedValue = ConvertValueToPropertyType(propertyInfo.PropertyType, property.Value);

                propertyInfo.SetValue(result, convertedValue);
            }

            return result;
        }

        private IDictionary<string, string> ParsePropertiesByAttribute(string content, Regex deserializationRegex)
        {
            var matchResults = deserializationRegex.Match(content);

            var output = new Dictionary<string, string>();

            if (matchResults.Success)
            {
                foreach (var groupName in deserializationRegex.GetGroupNames())
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
                throw new CannotDeserializeByRegexException(content, deserializationRegex);
            }

            return output;
        }

        private IList<string> SplitTableContentIntoRows(string tableContent, Regex splitRegex)
        {
            var output = new List<string>();

            if (splitRegex == null)
            {
                output.Add(tableContent);
                return output;
            }

            var parts = splitRegex.Split(tableContent).Where(i => i.Trim() != String.Empty).ToArray();

            for (int i = 0; i < parts.Length - 1; i += 2)
            {
                output.Add(parts[i] + parts[i + 1]);
            }

            return output;
        }
    }
}
