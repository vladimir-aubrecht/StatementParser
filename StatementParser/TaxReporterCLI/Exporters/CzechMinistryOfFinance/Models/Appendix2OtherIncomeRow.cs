using System;
using System.Xml.Serialization;

namespace TaxReporterCLI.Exporters.CzechMinistryOfFinance.Models
{
    [Serializable]
    public class Appendix2OtherIncomeRow
    {
        /// <summary>
        /// Výpočet dílčího základu daně z ostatních příjmů (§ 10 zákona) - Příjmy
        /// </summary>
        [XmlAttribute("prijmy10")]
        public decimal Income { get; set; }

        /// <summary>
        /// Výpočet dílčího základu daně z ostatních příjmů (§ 10 zákona) - Kód
        /// </summary>
        [XmlAttribute("kod10")]
        public string Code { get; set; }

        /// <summary>
        /// Výpočet dílčího základu daně z ostatních příjmů (§ 10 zákona) - Slovní popis druhu příjmu podle § 10 odst. 1 zákona
        /// </summary>
        [XmlAttribute("druh_prij10")]
        public string Description { get; set; }

        /// <summary>
        /// Výpočet dílčího základu daně z ostatních příjmů (§ 10 zákona) - Rozdíl (příjmy - výdaje)
        /// </summary>
        [XmlAttribute("rozdil10")]
        public decimal Profit { get; set; }

        /// <summary>
        /// Výpočet dílčího základu daně z ostatních příjmů (§ 10 zákona) - Výdaje
        /// </summary>
        [XmlAttribute("vydaje10")]
        public decimal Expenses { get; set; }

        /// <summary>
        /// Výpočet dílčího základu daně z ostatních příjmů (§ 10 zákona) - Označení druhu příjmů podle § 10 odst. 1 zákona
        /// </summary>
        [XmlAttribute("kod_dr_prij10")]
        public string Type { get; set; }
    }
}
