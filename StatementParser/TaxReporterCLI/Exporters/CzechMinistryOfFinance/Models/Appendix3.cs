using System.Xml.Serialization;

namespace TaxReporterCLI.Exporters.CzechMinistryOfFinance.Models
{
    public class Appendix3
    {
        [XmlAttribute("proczahr")]
        public double Coeficient { get; set; }

        [XmlAttribute("da_uznzap")]
        public decimal MaximumRecognizedTax { get; set; }

        [XmlAttribute("da_zahr")]
        public decimal Tax { get; set; }

        [XmlAttribute("kc_prijzap")]
        public decimal Income { get; set; }

        [XmlAttribute("roz_od12")]
        public decimal UnrecognisedTax { get; set; }

        [XmlAttribute("kc_k_zapzahr")]
        public decimal RecognizedTax { get; set; }

        [XmlAttribute("kod_statu")]
        public string Country { get; set; }
    }
}
