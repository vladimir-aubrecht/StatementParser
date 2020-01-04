using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using StatementParser.Parsers.Pdf.Exceptions;

namespace StatementParser.Parsers.Pdf
{
    internal class PdfTable<TRowDescriptor> : IEnumerable<PdfTableRow<TRowDescriptor>> where TRowDescriptor : new()
    {
        private IList<PdfTableRow<TRowDescriptor>> rows;

        public PdfTableRow<TRowDescriptor> this[int index]
        {
            get
            {
                return rows[index];
            }
        }

        public PdfTable(string tableContent)
        {
            this.rows = ConvertToPdfTableRows(SplitTableContentIntoRows(tableContent));
        }

        public IEnumerator<PdfTableRow<TRowDescriptor>> GetEnumerator()
        {
            return rows.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IList<PdfTableRow<TRowDescriptor>> ConvertToPdfTableRows(IList<string> stringRows)
        {
            return stringRows.Select(i =>
                {
                    try
                    {
                        return new PdfTableRow<TRowDescriptor>(i);
                    }
                    catch (CannotParseRowException)
                    {
                        // We support only rows which match regex, all other are dropped.
                        return null;
                    }
                }
             ).Where(i => i != null).ToList();
        }

        private IList<string> SplitTableContentIntoRows(string tableContent)
        {
            var attribute = typeof(TRowDescriptor).GetCustomAttribute<DeserializeByRegexAttribute>(true)
                ?? throw new InvalidOperationException($"Class {nameof(TRowDescriptor)} must use {nameof(DeserializeByRegexAttribute)} attribute.");

            var output = new List<string>();

            if (attribute.RowSplitRegex == null)
            {
                output.Add(tableContent);
                return output;
            }

            var parts = attribute.RowSplitRegex.Split(tableContent).Where(i => i.Trim() != String.Empty).ToArray();

            for (int i = 0; i < parts.Length - 1; i += 2)
            {
                output.Add(parts[i] + parts[i + 1]);
            }

            return output;
        }
    }
}
