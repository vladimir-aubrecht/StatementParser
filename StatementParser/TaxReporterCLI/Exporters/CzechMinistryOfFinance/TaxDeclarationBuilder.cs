
using TaxReporterCLI.Exporters.CzechMinistryOfFinance.Models;

namespace TaxReporterCLI.Exporters.CzechMinistryOfFinance
{
    public class TaxDeclarationBuilder
    {

        public Declarations Build()
        {
            return new Declarations()
            {
                TaxDeclaration = new TaxDeclaration()
                {
                    Taxes = new Taxes()
                    {
                        UnknownConstantField2 = "Z",
                        Year = "2019",
                        TaxPeriodStart = "01.01.2019",
                        TaxPeriodEnds = "31.12.2019",
                        DestinationMinistryOfFinanceId = "464",

                        SpouseTitle = "64",
                        SpouseFirstname = "63",
                        SpouseLastname = "62",
                        SpouseBirthnumber = "65",
                        SpouseBirthDate = "06.03.2020",

                        Row3 = "B",
                        Row5 = "N",
                        Row5a = "N",
                        Row30 = "N",
                        Row58 = "55.00",
                        Row60 = "55",
                        Row61 = "0",
                        Row62 = "60",
                        Row63 = "61",
                        Row64 = "455986",
                        Row65aMonthsCell = "68",
                        Row65aValueCell = "140760",
                        Row65bMonthsCell = "70",
                        Row65bValueCell = "289800",
                        Row66MonthsCell = "72",
                        Row66ValueCell = "73",
                        Row67MonthsCell = "74",
                        Row67ValueCell = "75",
                        Row68MonthsCell = "76",
                        Row68ValueCell = "77",
                        Row69MonthsCell = "78",
                        Row69ValueCell = "79",
                        Row69a = "80",
                        Row69b = "81",
                        Row70 = "24840",
                        Row71 = "0",
                        Row74 = "0",
                        Row84 = "84",
                        Row85 = "85",
                        Row86 = "86",
                        Row87 = "87",
                        Row87a = "88",
                        Row87b = "89",
                        Row88 = "90",
                        Row89 = "91",
                        Row90 = "92",
                        Row91 = "-792"
                    },
                    PersonalInformation = new PersonalInformation()
                    {
                        BirthLastname = "Aubrecht",
                        BirthNumber = "0000000000",
                        City = "PRAHA 4",
                        CurrentCityId = "500119",
                        TaxYearCityId = "500119",
                        TaxYearLastDayCityId = "500119",
                        CurrentCountry = "ČESKÁ REPUBLIKA",
                        CurrentEmail = "vladimir.aubrecht@me.com",
                        TaxYearEmail = "vladimir.aubrecht@me.com",
                        CurrentFaxNumber = null,
                        TaxYearFaxNumber = null,
                        CurrentPhone = "+420606000000",
                        TaxYearPhone = "+420606000000",
                        CurrentStreet = "Ulice",
                        TaxYearStreet = "Ulice",
                        TaxYearLastDayStreet = "Ulice",
                        Firstname = "Vladimír",
                        Lastname = "Aubrecht",
                        CurrentZip = "14000",
                        TaxYearLastDayZip = "14000",
                        TaxYearZip = "14000",
                        TaxCountry = "CZ",
                        Nationality = "ČR",
                        TaxNumber = null,
                        TaxYearCity = "PRAHA 4",
                        TaxYearLastDayCity = "PRAHA 4",
                        PassportNumber = null,
                        Title = "Ing.",
                        TaxYearLastDayStreetDescriptionNumber = "000",
                        CurrentStreetDescriptionNumber = "000",
                        TaxYearStreetDescriptionNumber = "000",
                        CurrentStreetNumber = "000",
                        TaxYearLastDayStreetNumber = "000",
                        TaxYearStreetNumber = "000",
                        MinistryOfFinanceId = "2004"

                    },
                    Section2 = new Section2()
                    {
                        Row31 = "24",
                        Row32 = "25",
                        Row33 = "26",
                        Row34 = "27",
                        Row35 = "28",
                        Row36 = "29",
                        Row36a = "30",
                        Row37 = "31",
                        Row38 = "32",
                        Row39 = "33",
                        Row40 = "34",
                        Row41 = "35",
                        Row41a = "36",
                        Row42 = "37",
                        Row43 = "38",
                        Row44 = "39",
                        Row45 = "40"
                    },
                    Appendix2 = new Appendix2[]
                    {
                        new Appendix2() { Code = "Z", Type = "D", Description = "Příjmy z prodeje indexů", Expenses = "3919", Income = "20500", Profit = "16581" }
                    },
                    AppendixIncomeTable = new AppendixIncomeTable[]
                    {
                        new AppendixIncomeTable() { Income = "1791494", Insurance = "552066", TakenDeposit = "345159" },
                        new AppendixIncomeTable() { Income = "378327" },
                        new AppendixIncomeTable() { Income = "27372" }
                    },
                    Appendix3 = new Appendix3[]
                    {
                        new Appendix3() { Coeficient = 0.01, Country = "PL", Income = 263, MaximumRecognizedTax = 46.33M, RecognizedTax = 46.33M, Tax = 50, UnrecognisedTax = 3.67M }
                    },
                    AppendixSeznam = new AppendixSeznam[]
                    {
                        new AppendixSeznam() { BrokerName = "LYNX B.V.", Country = "PL", IncomeInCZK = 263, TaxInCZK = 50, TaxInOriginalCurrency = 8 },
                        new AppendixSeznam() { BrokerName = "RaiffeisenBank", Country = "US", IncomeInCZK = 4932, TaxInCZK = 740, TaxInOriginalCurrency = 32 }
                    }
                }
            };
        }
    }
}
