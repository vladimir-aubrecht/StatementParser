using System;
using System.Xml.Serialization;

namespace TaxReporterCLI.Exporters.CzechMinistryOfFinance.Models
{
    [Serializable]
    public class AppendixSeznam
    {
        /// <summary>
        /// 4. daňpokyny k vyplnění 
        /// </summary>
        [XmlAttribute("dan_seznam")]
        public int TaxInCZK { get; set; }

        /// <summary>
        /// 3. zaplacená daňpokyny k vyplnění 
        /// </summary>
        [XmlAttribute("zapl_dan")]
        public int TaxInOriginalCurrency { get; set; }

        /// <summary>
        /// 2. stát zdroje příjmůpokyny k vyplnění
        /// </summary>
        [XmlAttribute("k_stat_zdroj")]
        public string Country { get; set; }

        /// <summary>
        /// 1. identifikační údaje (adresa)pokyny k vyplnění 
        /// </summary>
        [XmlAttribute("ident_udaje")]
        public string BrokerName { get; set; }

        /// <summary>
        /// 5. příjmypokyny k vyplnění 
        /// </summary>
        [XmlAttribute("prijmy_seznam")]
        public int IncomeInCZK { get; set; }
    }
}
