using System;
using System.Xml.Serialization;

namespace TaxReporterCLI.Exporters.CzechMinistryOfFinance.Models
{
    [Serializable]
    public class AppendixIncomeTable
    {
        [XmlAttribute("kc_poj6p")]
        public string Insurance { get; set; }

        [XmlAttribute("kc_zalzavcp")]
        public string TakenDeposit { get; set; }

        [XmlAttribute("kc_prij6p")]
        public string Income { get; set; }
    }
}
