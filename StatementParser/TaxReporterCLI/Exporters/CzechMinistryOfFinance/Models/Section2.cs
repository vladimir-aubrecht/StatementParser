using System;
using System.Xml.Serialization;

namespace TaxReporterCLI.Exporters.CzechMinistryOfFinance.Models
{
    [Serializable]
    public class Section2
    {
        /// <summary>
        /// Úhrn dílčích základů daně podle § 7 až § 10 zákona po vynětí (ř. 41 – úhrn vyňatých příjmů ze zdrojů v zahraničí podle § 7 až § 10 zákona nebo ř. 41) 
        /// </summary>
        /// <remarks>
        /// Row 41a.
        /// </remarks>
        [XmlAttribute("kc_vynprij")]
        public string Row41a { get; set; }

        /// <summary>
        /// Úhrn dílčích základů daně podle § 7 až § 10 zákona po vynětí (ř. 41 - úhrn vyňatých příjmů ze zdrojů v zahraničí podle § 7 až § 10 zákona nebo ř. 41)
        /// </summary>
        /// <remarks>
        /// Row 41.
        /// </remarks>
        [XmlAttribute("kc_uhrn")]
        public string Row41 { get; set; }

        /// <summary>
        /// Základ daně po odečtení ztráty (ř. 42 – ř. 44)
        /// </summary>
        /// <remarks>
        /// Row 45.
        /// </remarks>
        [XmlAttribute("kc_zakldan")]
        public string Row45 { get; set; }

        /// <summary>
        /// Dílčí základ daně podle § 6 zákona (ř. 31 + ř. 32 – ř. 33)
        /// </summary>
        /// <remarks>
        /// Row 34.
        /// </remarks>
        [XmlAttribute("kc_zd6p")]
        public string Row34 { get; set; }

        /// <summary>
        /// Úhrn příjmů od všech zaměstnavatelů
        /// </summary>
        /// <remarks>
        /// Row 31.
        /// </remarks>
        [XmlAttribute("kc_prij6")]
        public string Row31 { get; set; }

        /// <summary>
        /// Díl čí základ daně z ostatních příjmů podle § 10 zákona (ř. 209 přílohy č. 2 DAP) 
        /// </summary>
        /// <remarks>
        /// Row 40.
        /// </remarks>
        [XmlAttribute("kc_zd10")]
        public string Row40 { get; set; }

        /// <summary>
        /// Úhrn příjmů plynoucí ze zahraničí zvýšený o povinné pojistné podle § 6 odst. 12 zákona
        /// </summary>
        /// <remarks>
        /// Row 35.
        /// </remarks>
        [XmlAttribute("kc_prij6zahr")]
        public string Row35 { get; set; }

        /// <summary>
        /// Dílčí základ daně nebo ztráta ze samostatné činnosti podle § 7 zákona (ř. 113 přílohy č. 1 DAP) 
        /// </summary>
        /// <remarks>
        /// Row 37.
        /// </remarks>
        [XmlAttribute("kc_zd7")]
        public string Row37 { get; set; }

        /// <summary>
        /// Dílčí základ daně ze závislé činnosti podle § 6 zákona (ř. 34) 
        /// </summary>
        /// <remarks>
        /// Row 36.
        /// </remarks>
        [XmlAttribute("kc_zd6")]
        public string Row36 { get; set; }

        /// <summary>
        /// Daň zaplacená v zahraničí podle § 6 odst. 13 zákona
        /// </summary>
        /// <remarks>
        /// Row 33.
        /// </remarks>
        [XmlAttribute("kc_dan_zah")]
        public string Row33 { get; set; }

        /// <summary>
        /// Dílčí základ daně ze závislé činnosti podle § 6 zákona po vynětí (ř. 36 – úhrn vyňatých příjmů ze zdrojů v zahraničí podle § 6 zákona nebo ř. 36)
        /// </summary>
        /// <remarks>
        /// Row 36a.
        /// </remarks>
        [XmlAttribute("kc_vynprij_6")]
        public string Row36a { get; set; }

        /// <summary>
        /// Úhrn povinného pojistného podle § 6 odst.12 zákona
        /// </summary>
        /// <remarks>
        /// Row 32.
        /// </remarks>
        [XmlAttribute("kc_poj6")]
        public string Row32 { get; set; }

        /// <summary>
        /// Dílčí základ daně z kapitálového majetku podle § 8 zákona 
        /// </summary>
        /// <remarks>
        /// Row 38.
        /// </remarks>
        [XmlAttribute("kc_zakldan8")]
        public string Row38 { get; set; }

        /// <summary>
        /// Dílčí základ daně nebo ztráta z nájmu podle § 9 zákona (ř. 206 přílohy č. 2 DAP)
        /// </summary>
        /// <remarks>
        /// Row 39.
        /// </remarks>
        [XmlAttribute("kc_zd9")]
        public string Row39 { get; set; }

        /// <summary>
        /// Základ daně (36a + kladná hodnota z ř. 41a) 
        /// </summary>
        /// <remarks>
        /// Row 42.
        /// </remarks>
        [XmlAttribute("kc_zakldan23")]
        public string Row42 { get; set; }

        /// <summary>
        /// Úhrn příjmů podle § 6 zákona od všech zaměstnavatelů po vynětí (ř. 31 – úhrn vyňatých příjmů podle § 6 zákona od všech zaměstnavatelů)
        /// </summary>
        /// <remarks>
        /// Row 43.
        /// </remarks>
        [XmlAttribute("kc_prij6vyn")]
        public string Row43 { get; set; }

        /// <summary>
        /// Uplatňovaná výše ztráty – vzniklé a vyměřené za předcházející zdaňovací období maximálně do výše ř. 41a 
        /// </summary>
        /// <remarks>
        /// Row 44.
        /// </remarks>
        [XmlAttribute("kc_ztrata2")]
        public string Row44 { get; set; }
    }
}
