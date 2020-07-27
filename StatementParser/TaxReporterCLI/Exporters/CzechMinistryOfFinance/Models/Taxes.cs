using System;
using System.Xml.Serialization;

namespace TaxReporterCLI.Exporters.CzechMinistryOfFinance.Models
{
    [Serializable]
    public class Taxes
    {
        /// <summary>
        /// Částka podle § 35ba odst. 1 písm. d) zákona (rozšířená sleva na invaliditu – pro poživatele invalidního důchodu pro invaliditu třetího stupně)
        /// </summary>
        /// <remarks>
        /// Row 67 value cell.
        /// </remarks>
        [XmlAttribute("kc_op15_1e1")]
        public string Row67ValueCell { get; set; }

        /// <summary>
        /// If you know what this is, please create PR or just write me an email (vladimir.aubrecht@me.com)!
        /// </summary>
        [XmlAttribute("uv_rozsah")]
        public string UnknownConstantField2 { get; set; }

        /// <summary>
        /// Daň po uplatnění slev podle § 35, § 35a, 35b, a § 35ba zákona (ř. 60 - ř. 70)
        /// </summary>
        /// <remarks>
        /// Row 71.
        /// </remarks>
        [XmlAttribute("da_slevy35ba")]
        public string Row71 { get; set; }

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
        public string Row5 { get; set; }

        /// <summary>
        /// Částka podle § 35ba odst. 1 písm. c) zákona (základní sleva na invaliditu – pro poživatele invalidního důchodu pro invaliditu prvního nebo druhého stupně)
        /// </summary>
        /// <remarks>
        /// Row 66 months cell.
        /// </remarks>
        [XmlAttribute("m_cinvduch")]
        public string Row66MonthsCell { get; set; }

        /// <summary>
        /// If you know what this is, please create PR or just write me an email (vladimir.aubrecht@me.com)!
        /// </summary>
        [XmlAttribute("dokument")]
        public string UnknownConstantField0 { get; set; } = "DP5";

        /// <summary>
        /// Zákonná povinnost ověření účetní závěrky auditorem
        /// </summary>
        /// <remarks>
        /// Row 5a.
        /// </remarks>
        [XmlAttribute("audit")]
        public string Row5a { get; set; }

        /// <summary>
        /// Částka podle § 35ba odst. 1 písm. b) zákona (sleva na manželku/manžela,  která/který je držitelem ZTP/P)
        /// </summary>
        /// <remarks>
        /// Row 65b value cell.
        /// </remarks>
        [XmlAttribute("kc_manztpp")]
        public string Row65bValueCell { get; set; }

        /// <summary>
        /// Finančnímu úřadu pro / Specializovanému finančnímu úřadu
        /// </summary>
        [XmlAttribute("c_ufo_cil")]
        public string DestinationMinistryOfFinanceId { get; set; }

        /// <summary>
        /// Section 5, table 1, spouse firstname
        /// </summary>
        [XmlAttribute("manz_jmeno")]
        public string SpouseFirstname { get; set; }

        /// <summary>
        /// Sražená daň podle § 36 odst. 6 zákona (státní dluhopisy)
        /// </summary>
        /// <remarks>
        /// Row 87.
        /// </remarks>
        [XmlAttribute("kc_sraz367")]
        public string Row87 { get; set; }


        /// <summary>
        /// If you know what this is, please create PR or just write me an email (vladimir.aubrecht@me.com)!
        /// </summary>
        [XmlAttribute("k_uladis")]
        public string UnknownConstantField1 { get; set; } = "DPF";

        /// <summary>
        /// Slevy celkem podle § 35 odst. 1 zákona 
        /// </summary>
        /// <remarks>
        /// Row 62.
        /// </remarks>
        [XmlAttribute("da_slevy")]
        public string Row62 { get; set; }

        /// <summary>
        /// Za zdaňovací období (kalendářní rok)
        /// </summary>
        [XmlAttribute("zdobd_od")]
        public string TaxPeriodStart { get; set; }

        /// <summary>
        /// Section 5, table 1, spouse lastname
        /// </summary>
        [XmlAttribute("manz_prijmeni")]
        public string SpouseLastname { get; set; }

        /// <summary>
        /// Částka podle § 35ba odst. 1 písm. c) zákona (základní sleva na invaliditu – pro poživatele invalidního důchodu pro invaliditu prvního nebo druhého stupně)
        /// </summary>
        /// <remarks>
        /// Row 66 value cell.
        /// </remarks>
        [XmlAttribute("kc_op15_1d")]
        public string Row66ValueCell { get; set; }

        /// <summary>
        /// Zbývá doplatit (ř. 74 - ř. 77 - ř. 84 - ř. 85 - ř. 86 - ř. 87 - ř. 87a - ř. 87b - ř. 88 - ř. 89 - ř. 90)
        /// </summary>
        /// <remarks>
        /// Row 91.
        /// </remarks>
        [XmlAttribute("kc_zbyvpred")]
        public string Row91 { get; set; }

        /// <summary>
        /// Částka podle § 35ba odst. 1 písm. b) zákona (sleva na manželku/manžela)
        /// </summary>
        /// <remarks>
        /// Row 65a value cell.
        /// </remarks>
        [XmlAttribute("kc_op15_1c")]
        public string Row65aValueCell { get; set; }

        /// <summary>
        /// Částka podle § 35ba odst. 1 písm. e) zákona (sleva na držitele průkazu ZTP/P)
        /// </summary>
        /// <remarks>
        /// Row 68 months cell.
        /// </remarks>
        [XmlAttribute("m_ztpp")]
        public string Row68MonthsCell { get; set; }

        /// <summary>
        /// Daň celkem zaokrouhlená na celé Kč nahoru (ř. 58 + ř. 59)
        /// </summary>
        /// <remarks>
        /// Row 60.
        /// </remarks>
        [XmlAttribute("da_celod13")]
        public string Row60 { get; set; }

        /// <summary>
        /// Písm. a) zákona (základní sleva na poplatníka (2013: na poplatníka))
        /// </summary>
        /// <remarks>
        /// Row 64.
        /// </remarks>
        [XmlAttribute("uhrn_slevy35ba")]
        public string Row64 { get; set; }

        /// <summary>
        /// Částka podle § 35ba odst. 1 písm. f) zákona (sleva na studenta) 
        /// </summary>
        /// <remarks>
        /// Row 69 months cell.
        /// </remarks>
        [XmlAttribute("m_stud")]
        public string Row69MonthsCell { get; set; }

        /// <summary>
        /// Zaplacená daň stanovená paušální částkou podle § 7a zákona 
        /// </summary>
        /// <remarks>
        /// Row 86.
        /// </remarks>
        [XmlAttribute("kc_pausal")]
        public string Row86 { get; set; }

        /// <summary>
        /// Úhrn sražených záloh na daň z příjmů fyzických osob ze závislé činnosti (po slevách na dani)
        /// </summary>
        /// <remarks>
        /// Row 84.
        /// </remarks>
        [XmlAttribute("kc_zalzavc")]
        public string Row84 { get; set; }

        /// <summary>
        /// Sražená daň podle § 36 odst. 8 zákona
        /// </summary>
        /// <remarks>
        /// Row 87b.
        /// </remarks>
        [XmlAttribute("kc_sraz_rezehp")]
        public string Row87b { get; set; }

        /// <summary>
        /// Úhrn slev na dani podle § 35, § 35a, § 35b a § 35ba zákona (ř. 62 + ř. 63 + ř. 64 + ř. 65a + ř. 65b + ř. 66 + ř. 67 + ř. 68 + ř. 69 + ř. 69a + ř. 69b)
        /// </summary>
        /// <remarks>
        /// Row 70.
        /// </remarks>
        [XmlAttribute("kc_op15_1a")]
        public string Row70 { get; set; }

        /// <summary>
        /// Transakce uskutečněné se zahraničními spojenými osobami (2013: Spojení se zahraničními osobami)
        /// </summary>
        /// <remarks>
        /// Row 30.
        /// </remarks>
        [XmlAttribute("prop_zahr")]
        public string Row30 { get; set; }

        /// <summary>
        /// Na zbývajících zálohách zaplaceno poplatníkem celkem
        /// </summary>
        /// <remarks>
        /// Row 85.
        /// </remarks>
        [XmlAttribute("kc_zalpred")]
        public string Row85 { get; set; }

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
        public string Row61 { get; set; }

        /// <summary>
        /// Zaplacená daňová povinnost (záloha) podle § 38gb odst. 2 zákona 
        /// </summary>
        /// <remarks>
        /// Row 90.
        /// </remarks>
        [XmlAttribute("kc_konkurs")]
        public string Row90 { get; set; }

        /// <summary>
        /// Section 5, table 1, spouse birth number
        /// </summary>
        [XmlAttribute("manz_r_cislo")]
        public string SpouseBirthnumber { get; set; }

        /// <summary>
        /// Částka podle § 35ba odst. 1 písm. b) zákona (sleva na manželku/manžela)
        /// </summary>
        /// <remarks>
        /// Row 65a months cell.
        /// </remarks>
        [XmlAttribute("m_vyzmanzl")]
        public string Row65aMonthsCell { get; set; }

        /// <summary>
        /// Daň po uplatnění slevy podle § 35c zákona (ř. 71 - ř. 73)
        /// </summary>
        /// <remarks>
        /// Row 74.
        /// </remarks>
        [XmlAttribute("da_slevy35c")]
        public string Row74 { get; set; }

        /// <summary>
        /// Sražená daň podle § 38f odst. 12 zákona 
        /// </summary>
        /// <remarks>
        /// Row 89.
        /// </remarks>
        [XmlAttribute("kc_sraz3810")]
        public string Row89 { get; set; }

        /// <summary>
        /// Částka podle § 35ba odst. 1 písm. f) zákona (sleva na studenta) 
        /// </summary>
        /// <remarks>
        /// Row 69 value cell.
        /// </remarks>
        [XmlAttribute("kc_stud")]
        public string Row69ValueCell { get; set; }

        /// <summary>
        /// Daň podle § 16 zákona (ř. 57) nebo částka z ř. 330 přílohy č. 3 DAP
        /// </summary>
        /// <remarks>
        /// Row 58.
        /// </remarks>
        [XmlAttribute("da_slezap")]
        public string Row58 { get; set; }

        /// <summary>
        /// Částka podle § 35ba odst. 1 písm. b) zákona (sleva na manželku/manžela,  která/který je držitelem ZTP/P)
        /// </summary>
        /// <remarks>
        /// Row 65b months cell.
        /// </remarks>
        [XmlAttribute("m_manz")]
        public string Row65bMonthsCell { get; set; }

        /// <summary>
        /// Sleva podle § 35a nebo § 35b zákona 
        /// </summary>
        /// <remarks>
        /// Row 63.
        /// </remarks>
        [XmlAttribute("sleva_rp")]
        public string Row63 { get; set; }

        /// <summary>
        /// Zajištěná daň plátcem podle § 38e zákona 
        /// </summary>
        /// <remarks>
        /// Row 88.
        /// </remarks>
        [XmlAttribute("kc_sraz385")]
        public string Row88 { get; set; }

        /// <summary>
        /// Částka podle § 35ba odst. 1 písm. d) zákona (rozšířená sleva na invaliditu – pro poživatele invalidního důchodu pro invaliditu třetího stupně)
        /// </summary>
        /// <remarks>
        /// Row 67 months cell.
        /// </remarks>
        [XmlAttribute("m_invduch")]
        public string Row67MonthsCell { get; set; }

        /// <summary>
        /// Section 5, table 1, spouse title
        /// </summary>
        [XmlAttribute("manz_titul")]
        public string SpouseTitle { get; set; }

        /// <summary>
        /// DAP
        /// </summary>
        /// <remarks>
        /// Row 3.
        /// </remarks>
        [XmlAttribute("dap_typ")]
        public string Row3 { get; set; }

        /// <summary>
        /// Solidární zvýšení daně podle § 16a zákona
        /// </summary>
        /// <remarks>
        /// Row 59.
        /// </remarks>
        [XmlAttribute("kc_solidzvys")]
        public string Row59 { get; set; }

        /// <summary>
        /// Částka podle § 35ba odst. 1 písm. e) zákona (sleva na držitele průkazu ZTP/P)
        /// </summary>
        /// <remarks>
        /// Row 68 value cell.
        /// </remarks>
        [XmlAttribute("kc_op15_1e2")]
        public string Row68ValueCell { get; set; }

        /// <summary>
        /// Částka podle § 35ba odst. 1 písm. g) zákona (sleva za umístění dítěte)
        /// </summary>
        /// <remarks>
        /// Row 69a
        /// </remarks>
        [XmlAttribute("kc_dite_ms")]
        public string Row69a { get; set; }

        /// <summary>
        /// Sražená daň podle § 36 odst. 7 zákona 
        /// </summary>
        /// <remarks>
        /// Row 87a.
        /// </remarks>
        [XmlAttribute("kc_sraz_6_4")]
        public string Row87a { get; set; }

        /// <summary>
        /// Section 5, table 1, spouse birth date
        /// </summary>
        [XmlAttribute("manz_d_nar")]
        public string SpouseBirthDate { get; set; }

        /// <summary>
        /// Částka podle § 35ba odst. 1 písm. h) zákona (sleva na evidenci tržeb)
        /// </summary>
        /// <remarks>
        /// Row 69b.
        /// </remarks>
        [XmlAttribute("kc_sleva_eet")]
        public string Row69b { get; set; }
    }
}
