using System;
using System.Xml.Serialization;

namespace TaxReporterCLI.Exporters.CzechMinistryOfFinance.Models
{
    [Serializable]
    public class AppendixIncomeTable
    {

        /// <summary>
        /// Úhrn povinného pojistného podle § 6 odst. 12 zákona (2013: 13 zákona)
        /// </summary>
        [XmlAttribute("kc_poj6p")]
        public string Insurance { get; set; }

        /// <summary>
        /// Sražená záloha na daň v úhrnné výši
        /// </summary>
        [XmlAttribute("kc_zalzavcp")]
        public string TakenDeposit { get; set; }

        /// <summary>
        /// Úhrn vyplacených měsíčních daňových bonusů podle § 35d ZDP
        /// </summary>
        [XmlAttribute("kc_vyplbonusp")]
        public string PaidMontlyBonuses { get; set; }

        /// <summary>
        /// Úhrn příjmů ze závislé činnosti (2013: a funkčních požitků)
        /// </summary>
        [XmlAttribute("kc_prij6p")]
        public string Income { get; set; }

        /// <summary>
        /// Sražená daň podle § 36 odst. 7 (2014: nebo 8) ZDP
        /// </summary>
        [XmlAttribute("kc_srazp")]
        public string TakenTaxByArticle36Paragraph7 { get; set; }

        /// <summary>
        /// Sražená daň podle § 36 odst. 8 ZDP
        /// </summary>
        [XmlAttribute("kc_sraz368p")]
        public string TakenTaxByArticle36Paragraph8 { get; set; }
    }
}
