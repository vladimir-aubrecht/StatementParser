using System;
using System.Linq;

namespace StatementParser.Parsers.Pdf.Extensions
{
	internal static class TypeExtensions
	{
		public static Type GetCollectionElementType(this Type collectionType)
		{
			if (collectionType.HasElementType)
			{
				return collectionType.GetElementType();
			}

			var types =
				(from method in collectionType.GetMethods()
				 where method.Name == "get_Item"
				 select method.ReturnType
				).Distinct().ToArray();

			if (types.Length == 0)
			{
				return null;
			}

			if (types.Length != 1)
			{
				throw new Exception(string.Format("{0} has multiple item types", collectionType.FullName));
			}

			return types[0];
		}
	}
}
