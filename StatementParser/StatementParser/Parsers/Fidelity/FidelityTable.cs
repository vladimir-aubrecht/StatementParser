using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace StatementParser.Parsers.Fidelity
{
    internal class FidelityTable : IEnumerable<string>
    {
        private static readonly Regex DefaultSplitRegex = new Regex("([0-9]{2}/[0-9]{2} )", RegexOptions.Compiled);
        private static readonly Regex PeriodSplitRegex = new Regex("([0-9]{2}/[0-9]{2}/[0-9]{4}-[0-9]{2}/[0-9]{2}/[0-9]{4})", RegexOptions.Compiled);

        private static readonly IDictionary<TableName, Regex> rowSplitRegexes = new Dictionary<TableName, Regex>
        {
            [TableName.ActivityBuy] = DefaultSplitRegex,
            [TableName.ActivityDividend] = DefaultSplitRegex,
            [TableName.ActivityOther] = DefaultSplitRegex,
            [TableName.ActivityTaxes] = DefaultSplitRegex,
            [TableName.SummaryESPP] = PeriodSplitRegex
        };

        private IList<string> rows;

        public string this[int index]
        {
            get
            {
                return rows[index];
            }
        }

        public FidelityTable(string tableContent, TableName tableName)
        {
            this.rows = SplitTableContentIntoRows(tableContent, tableName);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return rows.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static IList<string> SplitTableContentIntoRows(string tableContent, TableName tableName)
        {
            var parts = rowSplitRegexes[tableName].Split(tableContent).Where(i => i.Trim() != String.Empty).ToArray();

            var output = new List<string>();

            for (int i = 0; i < parts.Length - 1; i += 2)
            {
                output.Add(parts[i] + parts[i + 1]);
            }

            return output;
        }
    }
}
