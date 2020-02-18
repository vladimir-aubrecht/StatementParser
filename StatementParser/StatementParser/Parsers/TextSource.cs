using System;
using System.Collections.Generic;
using System.Linq;
using ASoft.TextDeserializer;

namespace StatementParser.Parsers
{
	internal class TextSource : ITextSource
	{
		private readonly UglyToad.PdfPig.PdfDocument document;

		public TextSource(UglyToad.PdfPig.PdfDocument document)
		{
			this.document = document ?? throw new ArgumentNullException(nameof(document));
		}

		public IEnumerable<string> GetPagesText()
		{
			return this.document.GetPages().Select(i => i.Text);
		}
	}
}
