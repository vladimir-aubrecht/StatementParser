using System;
using System.Collections.Generic;
using System.Reflection;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

namespace StatementParser.Parsers.Brokers.Lynx.Extensions
{
	public static class CsvReaderExtensions
	{
		public static T ReadObject<T>(this CsvReader csvReader, int headerFieldIndex, Func<bool> isHeaderFunc) where T : class
		{
			var output = (T)Activator.CreateInstance(typeof(T));
			var dataSets = GetDataSetsProperties(output);

			while (csvReader.Read())
			{
				if (isHeaderFunc())
				{
					csvReader.ReadHeader();
					continue;
				}

				if (csvReader.Context.HeaderRecord == null)
				{
					return null;
				}

				var key = csvReader.Context.HeaderRecord[headerFieldIndex];

				if (!dataSets.ContainsKey(key))
				{
					continue;
				}

				// TODO: refactor this code so it supports more than just List<>.
				var listType = dataSets[key].PropertyType.GenericTypeArguments[0];
				var record = csvReader.GetRecord(listType);

				object value = dataSets[key].GetValue(output);

				if (value == null)
				{
					value = Activator.CreateInstance(dataSets[key].PropertyType);
					dataSets[key].SetValue(output, value);
				}

				dataSets[key].PropertyType.GetMethod("Add").Invoke(value, new object[] { record });
			}
			return output;
		}

		private static Dictionary<string, PropertyInfo> GetDataSetsProperties<T>(T instance)
		{
			var properties = typeof(T).GetProperties();

			var output = new Dictionary<string, PropertyInfo>();
			foreach (var property in properties)
			{
				var attribute = property.GetCustomAttribute<NameAttribute>(false);

				if (attribute == null)
				{
					continue;
				}

				output.Add(attribute.Names[0], property);
			}

			return output;
		}
	}
}