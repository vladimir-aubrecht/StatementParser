
using System;
using CsvHelper.Configuration.Attributes;

namespace StatementParser.Parsers.Brokers.Lynx.CsvModels
{
    internal class StatementRowModel
    {
        [Index(0)]
        public string Statement {get; set; }
        
        [Index(2)]
        public string FieldName { get; set; }

        [Index(3)]
        public string FieldValue { get; set; }

        public override string ToString()
        {
            return $"{nameof(Statement)}: {Statement} {nameof(FieldName)}: {FieldName} {nameof(FieldValue)}: {FieldValue}";
        }
    }
}