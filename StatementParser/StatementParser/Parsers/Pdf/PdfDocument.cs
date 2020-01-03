using System;
using System.Collections.Generic;
using System.Linq;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace StatementParser.Parsers.Pdf
{
    internal class PdfDocument
    {
        private readonly UglyToad.PdfPig.PdfDocument document;
        private readonly IPdfConfiguration pdfConfiguration;

        public PdfTable this[PdfTableName tableName]
        {
            get
            {
                return new PdfTable(ParseTableContent(tableName), tableName, pdfConfiguration);
            }
        }

        public PdfDocument(UglyToad.PdfPig.PdfDocument document, IPdfConfiguration pdfConfiguration)
        {
            this.document = document ?? throw new ArgumentNullException(nameof(document));
            this.pdfConfiguration = pdfConfiguration ?? throw new ArgumentNullException(nameof(pdfConfiguration));
        }

        private string ParseTableContent(PdfTableName tableName)
        {
            var contents = GetPagesByHeader(this.document, tableName)
                .Select(page => ParseTransactionStringsFromPage(page, tableName));

            return String.Join("", contents); //Lets merge tables splitted cross multiple pages
        }

        private IList<Page> GetPagesByHeader(UglyToad.PdfPig.PdfDocument pdfDocument, PdfTableName tableName)
        {
            var output = new List<Page>();

            var header = this.pdfConfiguration.TableRestrictorsMap[tableName][Separators.Header];

            foreach (var page in pdfDocument.GetPages())
            {
                if (page.Text.Contains(header))
                {
                    output.Add(page);
                }
            }

            return output;
        }

        private string ParseTransactionStringsFromPage(Page page, PdfTableName tableName)
        {
            var header = this.pdfConfiguration.TableRestrictorsMap[tableName][Separators.Header];
            var footer = this.pdfConfiguration.TableRestrictorsMap[tableName][Separators.Footer];

            var startIndex = page.Text.IndexOf(header, StringComparison.Ordinal) + header.Length;
            var endIndex = page.Text.IndexOf(footer, StringComparison.Ordinal);

            return page.Text.Substring(startIndex, endIndex - startIndex);
        }
    }
}
