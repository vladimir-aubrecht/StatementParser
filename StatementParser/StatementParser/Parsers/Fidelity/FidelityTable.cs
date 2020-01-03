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

        private static readonly IDictionary<FidelityTableName, Regex> rowSplitRegexes = new Dictionary<FidelityTableName, Regex>
        {
            [FidelityTableName.ActivityBuy] = DefaultSplitRegex,
            [FidelityTableName.ActivityDividend] = DefaultSplitRegex,
            [FidelityTableName.ActivityOther] = DefaultSplitRegex,
            [FidelityTableName.ActivityTaxes] = DefaultSplitRegex,
            [FidelityTableName.SummaryESPP] = PeriodSplitRegex
        };

        private IList<string> rows;

        public string this[int index]
        {
            get
            {
                return rows[index];
            }
        }

        public FidelityTable(string tableContent, FidelityTableName tableName)
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

        private static IList<string> SplitTableContentIntoRows(string tableContent, FidelityTableName tableName)
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
