
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
                        CurrentStreet = "9",
                        CurrentZip = "11000",
                        CurrentEmail = "14",
                        TaxYearStreet = "18",
                        TaxYearLastDayCityId = "531278",
                        TaxYearEmail = "23",
                        TaxCountry = "CZ",
                        TaxYearLastDayStreetDescriptionNumber = "16",
                        BirthNumber = "2",
                        PassportNumber = "8",
                        Title = "6",
                        BirthLastname = "5",
                        TaxYearLastDayZip = "28543",
                        CurrentStreetDescriptionNumber = "10",
                        TaxYearZip = "56802",
                        CurrentCityId = "500054",
                        CurrentStreetNumber = "11",
                        TaxYearCityId = "572748",
                        CurrentPhone = "12",
                        TaxYearCity = "KARLE",
                        TaxYearLastDayCity = "PABĚNICE",
                        MinistryOfFinanceId = "3312",
                        Lastname = "3",
                        TaxYearFaxNumber = "22",
                        TaxYearPhone = "21",
                        CurrentFaxNumber = "13",
                        TaxYearStreetDescriptionNumber = "19",
                        CurrentCountry = "ČESKÁ REPUBLIKA",
                        TaxNumber = "1",
                        TaxYearLastDayStreet = "15",
                        Nationality = "7",
                        City = "PRAHA 1",
                        TaxYearStreetNumber = "20",
                        TaxYearLastDayStreetNumber = "17",
                        Firstname = "4",

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
                    Section3 = new Section3()
                    {
                        Row56 = "300",
                        Row49 = "44",
                        Row50 = "45",
                        Row54 = "52",
                        Row51 = "46",
                        Row46 = "41",
                        Row48 = "43",
                        Row47 = "42",
                        Row53MonthsCell = "50",
                        Row55 = "337",
                        Row53ValueCell = "51",
                        Row53DescriptionCell = "49",
                        Row57 = "45",
                        Row52 = "47",
                        Row52a = "48"
                    },
                    Appendix2 = new Appendix2[]
                    {
                        new Appendix2() { Code = "Z", Type = "D", Description = "94", Expenses = "96", Income = "95", Profit = "-1" }
                    },
                    AppendixIncomeTable = new AppendixIncomeTable[]
                    {
                        new AppendixIncomeTable() { Income = "101", Insurance = "102", TakenDeposit = "103", TakenTaxByArticle36Paragraph8 = "106", TakenTaxByArticle36Paragraph7 = "105", PaidMontlyBonuses = "104" },
                        new AppendixIncomeTable() { Income = "107", Insurance = "108", TakenDeposit = "109", TakenTaxByArticle36Paragraph8 = "112", TakenTaxByArticle36Paragraph7 = "111", PaidMontlyBonuses = "110" },
                    },
                    Appendix3 = new Appendix3[]
                    {
                        new Appendix3() { Coeficient = 0.00M, Country = "US", Income = 97, MaximumRecognizedTax = 0.00M, RecognizedTax = 0.00M, Tax = 99, UnrecognisedTax = 99.00M, Expenses = 98M }
                    },
                    AppendixSeznam = new AppendixSeznam[]
                    {
                        new AppendixSeznam() { BrokerName = "107", Country = "AZ", IncomeInCZK = 110, TaxInCZK = 109, TaxInOriginalCurrency = 108 },
                    },
                    AppendixOther = new AppendixOther()
                    {
                        AppendixText = "94"
                    },
                    AppendixSummary = new AppendixSummary()
                    {
                        TotalAmountOfAppendixes = "5",
                        TotalAmountOfAppendixes2 = "1",
                        TotalAmountOfAppendixes3 = "1",
                        TotalAmountOfAppendixesEmployeeIncomeConfirmation = "2",
                        TotalAmountOfAppendixesSeznam = "1"
                    }
                }
            };
        }
    }
}
