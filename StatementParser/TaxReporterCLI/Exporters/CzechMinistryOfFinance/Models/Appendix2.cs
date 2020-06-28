using System;
using System.Xml.Serialization;

namespace TaxReporterCLI.Exporters.CzechMinistryOfFinance.Models
{
    [Serializable]
    public class Appendix2
    {
        [XmlAttribute("prijmy10")]
        public string Income { get; set; }

        [XmlAttribute("kod10")]
        public string Code { get; set; }

        [XmlAttribute("druh_prij10")]
        public string Description { get; set; }

        [XmlAttribute("rozdil10")]
        public string Profit { get; set; }

        [XmlAttribute("vydaje10")]
        public string Expenses { get; set; }

        [XmlAttribute("kod_dr_prij10")]
        public string Type { get; set; }
    }
}
