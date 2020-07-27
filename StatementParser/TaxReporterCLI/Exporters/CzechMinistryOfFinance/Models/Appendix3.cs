using System.Xml.Serialization;

namespace TaxReporterCLI.Exporters.CzechMinistryOfFinance.Models
{
    public class Appendix3
    {
        /// <summary>
        /// Příjmy ze zdrojů v zahraničí - Vypočtená daň z příjmů ze zdrojů v zahraničí [(ř. 57 + ř. 59) - ř. 328]
        /// </summary>
        /// <remarks>Row 330.</remarks>
        [XmlAttribute("da_zazahr")]
        public decimal Row330 { get; set; }

        /// <summary>
        /// Příjmy ze zdrojů v zahraničí - Daň neuznaná k zápočtu (úhrn ř. 327)
        /// </summary>
        /// <remarks>Row 329.</remarks>
        [XmlAttribute("uhrn_neuzndan")]
        public decimal Row329 { get; set; }

        /// <summary>
        /// Příjmy ze zdrojů v zahraničí - Daň uznaná k zápočtu (úhrn ř. 326)
        /// </summary>
        /// <remarks>Row 328.</remarks>
        [XmlAttribute("uhrn_uzndan")]
        public decimal Row328 { get; set; }
    }
}
