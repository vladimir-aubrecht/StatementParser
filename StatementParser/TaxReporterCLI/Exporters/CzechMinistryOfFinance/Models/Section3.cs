using System;
using System.Xml.Serialization;

namespace TaxReporterCLI.Exporters.CzechMinistryOfFinance.Models
{
    [Serializable]
    public class Section3
    {
        /// <summary>
        /// Základ daně zaokrouhlený na celá sta Kč dolů
        /// </summary>
        /// <remarks>
        /// Row 56.
        /// </remarks>
        [XmlAttribute("kc_zdzaokr")]
        public string Row56 { get; set; }

        /// <summary>
        /// Odst. 6 zákona (soukromé životní pojištění (2013-2015: životní pojištění))
        /// </summary>
        /// <remarks>
        /// Row 49.
        /// </remarks>
        [XmlAttribute("kc_op15_13")]
        public string Row49 { get; set; }

        /// <summary>
        /// Odst. 7 zákona (odborové příspěvky)
        /// </summary>
        /// <remarks>
        /// Row 50.
        /// </remarks>
        [XmlAttribute("kc_op15_14")]
        public string Row50 { get; set; }

        /// <summary>
        /// Úhrn nezdanitelných částí základu daně a položek odčitatelných od základu daně (ř. 46 + ř. 47 + ř. 48 + ř. 49 + ř. 50 + ř. 51 + ř. 52 + ř. 52a + ř. 53) (2013: (ř. 46 + ř. 47 + ř. 48 + ř. 49 + ř. 50 + ř. 51 + ř. 52 + ř. 53))
        /// </summary>
        /// <remarks>
        /// Row 54.
        /// </remarks>
        [XmlAttribute("kc_odcelk")]
        public string Row54 { get; set; }

        /// <summary>
        /// Odst. 8 zákona (úhrada za zkoušky ověřující výsledky dalšího vzdělávání (2013-2015: úhrada za další vzdělávání))
        /// </summary>
        /// <remarks>
        /// Row 51.
        /// </remarks>
        [XmlAttribute("kc_dalsivzd")]
        public string Row51 { get; set; }

        /// <summary>
        /// Odst. 1 zákona (hodnota bezúplatného plnění - daru/darů)
        /// </summary>
        /// <remarks>
        /// Row 46.
        /// </remarks>
        [XmlAttribute("kc_op15_8")]
        public string Row46 { get; set; }

        /// <summary>
        /// Odst. 5 zákona (penzijní připojištění, penzijní pojištění a doplňkové penzijní spoření) (2013: penzijní připojištění)
        /// </summary>
        /// <remarks>
        /// Row 48.
        /// </remarks>
        [XmlAttribute("kc_op15_12")]
        public string Row48 { get; set; }

        /// <summary>
        /// Odst. 3 a 4 zákona (odečet úroků)
        /// </summary>
        /// <remarks>
        /// Row 47.
        /// </remarks>
        [XmlAttribute("kc_op28_5")]
        public string Row47 { get; set; }

        /// <summary>
        /// Další částky
        /// </summary>
        /// <remarks>
        /// Row 53 months cell.
        /// </remarks>
        [XmlAttribute("m_dalsi")]
        public string Row53MonthsCell { get; set; }

        /// <summary>
        /// Základ daně snížený o nezdanitelné části základu daně a položky odčitatelné od základu daně (ř. 45 - ř. 54)
        /// </summary>
        /// <remarks>
        /// Row 55.
        /// </remarks>
        [XmlAttribute("kc_zdsniz")]
        public string Row55 { get; set; }

        /// <summary>
        /// Další částky
        /// </summary>
        /// <remarks>
        /// Row 53 value cell.
        /// </remarks>
        [XmlAttribute("kc_op_dal")]
        public string Row53ValueCell { get; set; }

        /// <summary>
        /// Další částky
        /// </summary>
        /// <remarks>
        /// Row 53 description cell.
        /// </remarks>
        [XmlAttribute("text_op_dal")]
        public string Row53DescriptionCell { get; set; }

        /// <summary>
        /// Daň podle § 16 zákona
        /// </summary>
        /// <remarks>
        /// Row 57.
        /// </remarks>
        [XmlAttribute("da_dan16")]
        public string Row57 { get; set; }

        /// <summary>
        /// Částka podle § 34 odst. 4 zákona (výzkum a vývoj)
        /// </summary>
        /// <remarks>
        /// Row 52.
        /// </remarks>
        [XmlAttribute("kc_op34_4")]
        public string Row52 { get; set; }

        /// <summary>
        /// § 34 odst. 4 (odpočet na podporu odborného vzdělávání)
        /// </summary>
        /// <remarks>
        /// Row 52a.
        /// </remarks>
        [XmlAttribute("kc_podvzdel")]
        public string Row52a { get; set; }
    }
}
