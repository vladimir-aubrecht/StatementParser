using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StatementParser.Parsers.Pdf
{
    internal class PdfTable : IEnumerable<PdfTableRow>
    {
        private readonly IPdfConfiguration pdfConfiguration;
        private IList<PdfTableRow> rows;

        public PdfTableRow this[int index]
        {
            get
            {
                return rows[index];
            }
        }

        public PdfTable(string tableContent, PdfTableName tableName, IPdfConfiguration pdfConfiguration)
        {
            this.pdfConfiguration = pdfConfiguration ?? throw new ArgumentNullException(nameof(pdfConfiguration));
            this.rows = SplitTableContentIntoRows(tableContent, tableName).Select(i => new PdfTableRow(i)).ToList();
        }

        public IEnumerator<PdfTableRow> GetEnumerator()
        {
            return rows.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IList<string> SplitTableContentIntoRows(string tableContent, PdfTableName tableName)
        {
            var parts = this.pdfConfiguration.TableRowSplitRegexes[tableName].Split(tableContent).Where(i => i.Trim() != String.Empty).ToArray();

            var output = new List<string>();

            for (int i = 0; i < parts.Length - 1; i += 2)
            {
                output.Add(parts[i] + parts[i + 1]);
            }

            return output;
        }
    }
}
