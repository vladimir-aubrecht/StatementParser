using System;
using System.Xml.Serialization;

namespace TaxReporterCLI.Exporters.CzechMinistryOfFinance.Models
{
    [Serializable]
    public class AppendixSeznam
    {
        [XmlAttribute("dan_seznam")]
        public int TaxInCZK { get; set; }

        [XmlAttribute("zapl_dan")]
        public int TaxInOriginalCurrency { get; set; }

        [XmlAttribute("k_stat_zdroj")]
        public string Country { get; set; }

        [XmlAttribute("ident_udaje")]
        public string BrokerName { get; set; }

        [XmlAttribute("prijmy_seznam")]
        public int IncomeInCZK { get; set; }
    }
}
