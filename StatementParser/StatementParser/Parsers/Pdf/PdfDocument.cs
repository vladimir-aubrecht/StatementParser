using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace StatementParser.Parsers.Pdf
{
    internal class PdfDocument
    {
        private readonly UglyToad.PdfPig.PdfDocument document;

        public PdfDocument(UglyToad.PdfPig.PdfDocument document)
        {
            this.document = document ?? throw new ArgumentNullException(nameof(document));
        }

        public PdfTable<TRowDescriptor> ParseTable<TRowDescriptor>() where TRowDescriptor : new()
        {
            var attribute = typeof(TRowDescriptor).GetCustomAttribute<DeserializeByRegexAttribute>(true)
                ?? throw new InvalidOperationException($"Class {nameof(TRowDescriptor)} must use {nameof(DeserializeByRegexAttribute)} attribute.");

            return new PdfTable<TRowDescriptor>(ParseTableContent(attribute));
        }

        private string ParseTableContent(DeserializeByRegexAttribute deserializeByRegexAttribute)
        {
            var contents = GetPagesByHeader(this.document, deserializeByRegexAttribute)
                .Select(page => deserializeByRegexAttribute.PageBodyRegex.Match(page.Text).Groups[1].Value);

            return String.Join("", contents); //Lets merge tables splitted cross multiple pages
        }

        private IList<Page> GetPagesByHeader(UglyToad.PdfPig.PdfDocument pdfDocument, DeserializeByRegexAttribute deserializeByRegexAttribute)
        {
            var output = new List<Page>();

            foreach (var page in pdfDocument.GetPages())
            {
                if (deserializeByRegexAttribute.PageBodyRegex.Match(page.Text).Success)
                {
                    output.Add(page);
                }
            }

            return output;
        }
    }
}
