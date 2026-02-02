using ASoft.TextDeserializer.Attributes;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatementParser.Parsers.Brokers.Revolut.PdfModels
{
    //Bug: Symbol can contain digits and hyphens, thou in Revolut PnL Statement Pdf is impossible to find out where Symbol ends and Quantity column starts.
    //Due to this reason symplification is made and we expect that Symbol cannot have any digits. It's wrong but so far sounds as very rare case.
    [DeserializeByRegex("(?<DateAcquired>\\d{4}-\\d{2}-\\d{2})(?<DateSold>\\d{4}-\\d{2}-\\d{2})(?<Symbol>[A-Z]+)(?<SecurityName>[A-Z][^0-9]*?)(?<ISIN>US.{10})(?<Country>.{2})(?<Quantity>[^\\$]+?)U?S?\\$(?<CostBasis>[^\\$]+?)U?S?\\$(?<GrossProceeds>[^\\$]*[0-9])(?<PnL>-?U?S?\\$[0-9\\.]+-?[0-9\\.,]*)[^\\$]+(?<Fees>-?U?S?\\$[0-9\\.]+)")]
    internal class SaleModel
    {
        public DateTime DateAcquired { get; set; }
        public DateTime DateSold { get; set; }
        public string Symbol { get; set; }
        public string SecurityName { get; set; }
        public string ISIN { get; set; }
        public string Country { get; set; }
        public decimal Quantity { get; set; } 
        public string CostBasis { get; set; }  
        public string GrossProceeds { get; set; }
        public string Fees { get; set; }
        public string PnL { get; set; }

    }
}
