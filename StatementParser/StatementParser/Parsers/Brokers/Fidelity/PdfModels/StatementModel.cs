using System;
using StatementParser.Parsers.Pdf;

namespace StatementParser.Parsers.Brokers.Fidelity.PdfModels
{
    [DeserializeByRegex(
        deserializationRegexPattern: ".+, (?<Year>[0-9]{4})",
        pageBodyRegexPattern: "STOCK PLAN SERVICES REPORT(.+?)Envelope")]
    public class StatementModel
    {
        public int Year { get; set; }
    }
}
