using System.Collections.Generic;

namespace StatementParser.Parsers.Pdf
{
	internal interface IPdfSource
	{
		IEnumerable<string> GetPagesText();
	}
}
