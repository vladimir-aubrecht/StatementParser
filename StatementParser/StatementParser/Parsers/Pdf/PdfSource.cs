using System;
using System.Collections.Generic;
using System.Linq;

namespace StatementParser.Parsers.Pdf
{
    internal class PdfSource : IPdfSource
    {
        private readonly UglyToad.PdfPig.PdfDocument document;

        public PdfSource(UglyToad.PdfPig.PdfDocument document)
        {
            this.document = document ?? throw new ArgumentNullException(nameof(document));
        }

        public IEnumerable<string> GetPagesText()
        {
            return this.document.GetPages().Select(i => i.Text);
        }
    }
}
