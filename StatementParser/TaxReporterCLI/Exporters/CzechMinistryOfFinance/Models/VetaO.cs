using System;
using System.Xml.Serialization;

namespace TaxReporterCLI.Exporters.CzechMinistryOfFinance.Models
{
    [Serializable]
    public class VetaO
    {
        //kc_zd6p="2749259" kc_prij6="2197193" kc_zd10="16581" kc_prij6zahr="405699" kc_zd6="2749259" kc_vynprij_6="2749259" kc_poj6="552066" kc_zakldan8="53687" kc_zakldan23="2819527" kc_prij6vyn="2197193" />

        /// <summary>
        /// Úhrn řádků (ř. 37 + ř. 38 + ř. 39 + ř. 40).
        /// </summary>
        /// <remarks>
        /// Row 41.
        /// </remarks>
        [XmlAttribute("kc_vynprij")]
        public string SumOfRows37to40 { get; set; }

        /// <summary>
        /// Úhrn dílčích základů daně podle § 7 až § 10 zákona po vynětí (ř. 41 - úhrn vyňatých příjmů ze zdrojů v zahraničí podle § 7 až § 10 zákona nebo ř. 41)
        /// </summary>
        /// <remarks>
        /// Row 41a.
        /// </remarks>
        [XmlAttribute("kc_uhrn")]
        public string SumOfPartialTaxBases { get; set; }

        /// <summary>
        /// Základ daně (ř. 36a + kladná hodnota z ř. 41a)
        /// </summary>
        /// <remarks>
        /// Row 42.
        /// </remarks>
        [XmlAttribute("kc_zakldan")]
        public string TaxBase { get; set; }
    }
}
