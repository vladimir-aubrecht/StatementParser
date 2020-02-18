using System;
namespace StatementParser.Parsers.Pdf.Exceptions
{
	internal class PdfException : Exception
	{
		public PdfException(string message) : base(message)
		{

		}
	}
}