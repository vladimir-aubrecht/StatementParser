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
    [DeserializeByRegex("(?<DateAcquired>\\d{4}-\\d{2}-\\d{2})(?<DateSold>\\d{4}-\\d{2}-\\d{2})(?<Symbol>[A-Z]+)(?<SecurityName>[A-Z][^0-9]*?)(?<ISIN>US.{10})(?<Country>.{2})(?<Quantity>[^\\$]+)\\$(?<CostBasis>[^\\$]+)\\$(?<GrossProceeds>[^\\$]*[0-9])(?<PnLRaw>-?\\$[0-9\\.]+?)(?<Fees>-?\\$[0-9\\.]+?)")]
    internal class SaleModel
    {
        public DateTime DateAcquired { get; set; }
        public DateTime DateSold { get; set; }
        public string Symbol { get; set; }
        public string SecurityName { get; set; }
        public string ISIN { get; set; }
        public string Country { get; set; }
        public decimal Quantity { get; set; } 
        public decimal CostBasis { get; set; }  
        public decimal GrossProceeds { get; set; }
        public String Fees { get; set; }
        public String PnLRaw { get; set; }
        public decimal PnL => Convert.ToDecimal(PnLRaw.Replace("$", ""), CultureInfo.InvariantCulture);

    }
}
