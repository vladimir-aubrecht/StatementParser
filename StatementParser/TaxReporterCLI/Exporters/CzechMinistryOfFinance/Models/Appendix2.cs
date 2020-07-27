using System;
using System.Xml.Serialization;

namespace TaxReporterCLI.Exporters.CzechMinistryOfFinance.Models
{
    [Serializable]
    public class Appendix2
    {
        /// <summary>
        /// Dílčí základ daně připadající na ostatní příjmy podle § 10 zákona (ř. 207 - ř. 208)
        /// </summary>
        /// <remarks>Row 209.</remarks>
        [XmlAttribute("kc_zd10p")]
        public string Row209 { get; set; }

        /// <summary>
        /// Výpočet dílčího základu daně z ostatních příjmů (§ 10 zákona) - Úhrn jednotlivých výdajů dle § 10 zákona
        /// </summary>
        [XmlAttribute("uhrn_vydaje10")]
        public string TotalExpenses { get; set; }

        /// <summary>
        /// Výpočet dílčího základu daně z příjmů z nájmu (§ 9 zákona) - Dosáhl jsem příjmů ze společného jmění manželů
        /// </summary>
        [XmlAttribute("spol_jm_manz")]
        public string IncomesFromPartnerSharedWealth { get; set; }

        /// <summary>
        /// Výpočet dílčího základu daně z příjmů z nájmu (§ 9 zákona) - Uplatňuji výdaje procentem z příjmů (30 %)
        /// </summary>
        [XmlAttribute("vyd9proc")]
        public string ApplyForExpensesGivenByFixedPercentage { get; set; }

        /// <summary>
        /// Příjmy podle § 10 zákona
        /// </summary>
        /// <remarks>Row 207.</remarks>
        [XmlAttribute("kc_prij10")]
        public string Row207 { get; set; }

        /// <summary>
        /// Výpočet dílčího základu daně z ostatních příjmů (§ 10 zákona) - Úhrn kladných rozdílů jednotlivých druhů příjmů
        /// </summary>
        [XmlAttribute("uhrn_rozdil10")]
        public string TotalProfit { get; set; }

        /// <summary>
        /// Výdaje podle § 10 zákona (maximálně do výše příjmů)
        /// </summary>
        /// <remarks>Row 208.</remarks>
        [XmlAttribute("kc_vyd10")]
        public string Row208 { get; set; }

        /// <summary>
        /// Výpočet dílčího základu daně z ostatních příjmů (§ 10 zákona) - Úhrn jednotlivých příjmů dle § 10 zákona
        /// </summary>
        [XmlAttribute("uhrn_prijmy10")]
        public string TotalIncomes { get; set; }
    }
}
