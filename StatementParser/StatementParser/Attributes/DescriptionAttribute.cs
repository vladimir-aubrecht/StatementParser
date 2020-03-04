using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace StatementParser.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DescriptionAttribute : Attribute
    {
        private static readonly Regex PlaceHolderRegex = new Regex("\\{([^\\}]+)\\}", RegexOptions.Compiled);

        private string Description { get; }

        public DescriptionAttribute(string description)
        {
            this.Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        public static string ConstructDescription(PropertyInfo propertyInfo, object instanceHoldingProperty)
        {
            var attribute = propertyInfo.GetCustomAttribute<DescriptionAttribute>(true);

            if (attribute == null)
            {
                return propertyInfo.Name;
            }

            var output = attribute.Description;
            var matches = PlaceHolderRegex.Matches(attribute.Description);

            foreach (Match match in matches)
            {
                var propertyName = match.Groups[1].Value;

                var placeholderProperty = instanceHoldingProperty.GetType().GetProperty(propertyName);

                if (placeholderProperty == null)
                {
                    continue;
                }

                output = output.Replace($"{{{propertyName}}}", placeholderProperty.GetValue(instanceHoldingProperty).ToString());
            }

            return output;
        }
    }
}