using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASoft.TextDeserializer;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace StatementParser.Parsers
{
	internal class TextSource : ITextSource
	{
		private readonly PdfDocument document;
		private readonly bool improved;

		public TextSource(string filePath, bool improved = false)
		{
			_ = filePath ?? throw new ArgumentNullException(nameof(filePath));

			this.document = PdfDocument.Open(filePath);
			this.improved = improved;
		}

		public string Title
			=> this.document.Information.Title;

		public IEnumerable<string> GetPagesText()
		{
			if (!improved)
			{
				return this.document.GetPages().Select(i => i.Text);
			}
			else
			{
				return this.document.GetPages().Select(i => GetPageText(i));
			}
		}

		public void Dispose()
		{
			this.document.Dispose();
		}

		/// <summary>
		/// Like <c>Page.Text</c>, but tries to retain more of the original
		/// document's structure by adding whitespace like '\n', '\t' and ' '.
		/// </summary>
		private string GetPageText(Page page)
		{
			var sb = new StringBuilder();
			var lastBottom = 0.0;
			var lastRight = 0.0;

			foreach (var word in page.GetWords())
			{
				if (sb.Length > 0 && Math.Abs(word.BoundingBox.Bottom - lastBottom) > word.BoundingBox.Height / 2)
				{
					// The vertical position differs too much, separate to a new row
					sb.Append('\n');
				}
				else if (sb.Length > 0 && sb[^1] != '\n' && word.BoundingBox.Left - lastRight > word.BoundingBox.Height)
				{
					// The horizontal gap between the right edge of the previous word and the left edge of this word is too big, separate to a new column
					sb.Append('\t');
				}
				lastBottom = word.BoundingBox.Bottom;
				lastRight = word.BoundingBox.Right;

				if (sb.Length > 0 && !char.IsWhiteSpace(sb[^1]))
				{
					// Make sure there are spaces between words
					sb.Append(' ');
				}
				sb.Append(word.Text);
			}

			return sb.ToString();
		}
	}
}
