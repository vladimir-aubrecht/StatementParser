using StatementParser.Parsers.Pdf.Attributes;

namespace StatementParser.Parsers.Brokers.Fidelity.PdfModels
{
    [DeserializeByRegex("(?<Date>[0-9]{2}/[0-9]{2}) .+?Tax -\\$(?<Tax>[0-9]+\\.[0-9]{2})")]
    internal class ActivityTaxesModel
    {
        public string Date { get; set; }

        public decimal Tax { get; set; }
    }
}
