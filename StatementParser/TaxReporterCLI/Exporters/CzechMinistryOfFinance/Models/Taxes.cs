using System;
using System.Xml.Serialization;

namespace TaxReporterCLI.Exporters.CzechMinistryOfFinance.Models
{
    [Serializable]
    public class Taxes
    {
        /// <summary>
        /// Daň po uplatnění slev podle § 35, § 35a, 35b, a § 35ba zákona (ř. 60 - ř. 70)
        /// </summary>
        /// <remarks>
        /// Row 71.
        /// </remarks>
        [XmlAttribute("da_slevy35ba")]
        public string TaxAfterDiscounts { get; set; }

        /// <summary>
        /// Za zdaňovací období (kalendářní rok)
        /// </summary>
        [XmlAttribute("rok")]
        public string Year { get; set; }

        /// <summary>
        /// DAP zpracoval a předkládá daňový poradce na základě plné moci k zastupování, která byla podána správci daně před uplynutím neprodloužené lhůty
        /// </summary>
        /// <remarks>
        /// Row 5.
        /// </remarks>
        [XmlAttribute("pln_moc")]
        public string IsPowerOfAttorney { get; set; }

        /// <summary>
        /// If you know what this is, please create PR or just write me an email (vladimir.aubrecht@me.com)!
        /// </summary>
        [XmlAttribute("dokument")]
        public string Document { get; set; } = "DP5";

        /// <summary>
        /// Zákonná povinnost ověření účetní závěrky auditorem
        /// </summary>
        /// <remarks>
        /// Row 5a.
        /// </remarks>
        [XmlAttribute("audit")]
        public string IsAudit { get; set; }

        /// <summary>
        /// Finančnímu úřadu pro / Specializovanému finančnímu úřadu
        /// </summary>
        [XmlAttribute("c_ufo_cil")]
        public string DestinationMinistryOfFinanceId { get; set; }

        /// <summary>
        /// If you know what this is, please create PR or just write me an email (vladimir.aubrecht@me.com)!
        /// </summary>
        [XmlAttribute("k_uladis")]
        public string UnknownConstantField1 { get; set; } = "DPF";

        /// <summary>
        /// Za zdaňovací období (kalendářní rok)
        /// </summary>
        [XmlAttribute("zdobd_od")]
        public string TaxPeriodStart { get; set; }

        /// <summary>
        /// Zbývá doplatit (ř. 74 - ř. 77 - ř. 84 - ř. 85 - ř. 86 - ř. 87 - ř. 87a - ř. 87b - ř. 88 - ř. 89 - ř. 90)
        /// </summary>
        /// <remarks>
        /// Row 91.
        /// </remarks>
        [XmlAttribute("kc_zbyvpred")]
        public string TaxToPay { get; set; }

        /// <summary>
        /// Daň celkem zaokrouhlená na celé Kč nahoru (ř. 58 + ř. 59)
        /// </summary>
        /// <remarks>
        /// Row 60.
        /// </remarks>
        [XmlAttribute("da_celod13")]
        public string TotalTaxRounded { get; set; }

        /// <summary>
        /// Písm. a) zákona (základní sleva na poplatníka (2013: na poplatníka))
        /// </summary>
        /// <remarks>
        /// Row 64.
        /// </remarks>
        [XmlAttribute("uhrn_slevy35ba")]
        public string BasicDiscountForTaxPayer { get; set; }

        /// <summary>
        /// Úhrn sražených záloh na daň z příjmů fyzických osob ze závislé činnosti (po slevách na dani)
        /// </summary>
        /// <remarks>
        /// Row 84.
        /// </remarks>
        [XmlAttribute("kc_zalzavc")]
        public string PayedTax { get; set; }

        /// <summary>
        /// Úhrn slev na dani podle § 35, § 35a, § 35b a § 35ba zákona (ř. 62 + ř. 63 + ř. 64 + ř. 65a + ř. 65b + ř. 66 + ř. 67 + ř. 68 + ř. 69 + ř. 69a + ř. 69b)
        /// </summary>
        /// <remarks>
        /// Row 70.
        /// </remarks>
        [XmlAttribute("kc_op15_1a")]
        public string SumOfTaxDiscounts { get; set; }

        /// <summary>
        /// Transakce uskutečněné se zahraničními spojenými osobami (2013: Spojení se zahraničními osobami)
        /// </summary>
        /// <remarks>
        /// Row 30.
        /// </remarks>
        [XmlAttribute("prop_zahr")]
        public string DidTransactionsWithInternationalPeople { get; set; }

        /// <summary>
        /// Za zdaňovací období (kalendářní rok)
        /// </summary>
        [XmlAttribute("zdobd_do")]
        public string TaxPeriodEnds { get; set; }

        /// <summary>
        /// Daňová ztráta - zaokrouhlená na celé Kč nahoru bez znaménka mínus
        /// </summary>
        /// <remarks>
        /// Row 61.
        /// </remarks>
        [XmlAttribute("kc_dztrata")]
        public string TaxLostRounded { get; set; }

        /// <summary>
        /// Daň po uplatnění slevy podle § 35c zákona (ř. 71 - ř. 73)
        /// </summary>
        /// <remarks>
        /// Row 74.
        /// </remarks>
        [XmlAttribute("da_slevy35c")]
        public string TotalTaxAfterDiscounts { get; set; }

        /// <summary>
        /// Daň podle § 16 zákona (ř. 57) nebo částka z ř. 330 přílohy č. 3 DAP
        /// </summary>
        /// <remarks>
        /// Row 58.
        /// </remarks>
        [XmlAttribute("da_slezap")]
        public string TotalTax { get; set; }

        /// <summary>
        /// DAP
        /// </summary>
        /// <remarks>
        /// Row 3.
        /// </remarks>
        [XmlAttribute("dap_typ")]
        public string TaxDeclarationType { get; set; }

        /// <summary>
        /// Solidární zvýšení daně podle § 16a zákona
        /// </summary>
        /// <remarks>
        /// Row 59.
        /// </remarks>
        [XmlAttribute("kc_solidzvys")]
        public string SolidarityTax { get; set; }
    }
}
