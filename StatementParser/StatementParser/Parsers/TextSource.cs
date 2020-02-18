using System;
using System.Collections.Generic;
using System.Linq;
using ASoft.TextDeserializer;
using UglyToad.PdfPig;

namespace StatementParser.Parsers
{
	internal class TextSource : ITextSource
	{
		private readonly PdfDocument document;

		public TextSource(string filePath)
		{
			_ = filePath ?? throw new ArgumentNullException(nameof(filePath));

			this.document = PdfDocument.Open(filePath);
		}

		public IEnumerable<string> GetPagesText()
		{
			return this.document.GetPages().Select(i => i.Text);
		}

		public void Dispose()
		{
			this.document.Dispose();
		}
	}
}
