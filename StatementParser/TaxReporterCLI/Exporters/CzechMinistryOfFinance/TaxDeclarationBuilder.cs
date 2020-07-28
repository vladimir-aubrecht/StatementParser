
using System;

using TaxReporterCLI.Exporters.CzechMinistryOfFinance.Models;

namespace TaxReporterCLI.Exporters.CzechMinistryOfFinance
{
    public class TaxDeclarationBuilder
    {
        private static readonly Declaration defaultDeclaration = new Declaration()
        {
            TaxDeclaration = new TaxDeclaration()
            {
                Taxes = new Taxes()
                {
                }
            }
        };

        private DateTime taxPeriodStart = new DateTime(DateTime.Today.Year, 1, 1);

        private Declaration declaration;

        public TaxDeclarationBuilder() : this(defaultDeclaration)
        {

        }

        public TaxDeclarationBuilder(Declaration declaration)
        {
            this.declaration = declaration;
        }

        public void WithTaxYear(int year)
        {
            this.taxPeriodStart = new DateTime(year, 1, 1);
        }

        public Declaration Build()
        {
            declaration.TaxDeclaration.Taxes.Year = taxPeriodStart.Year;
            declaration.TaxDeclaration.Taxes.TaxPeriodStart = String.Format("{0:dd.MM.yyyy}", taxPeriodStart);
            declaration.TaxDeclaration.Taxes.TaxPeriodEnds = String.Format("{0:dd.MM.yyyy}", taxPeriodStart.AddYears(1).AddDays(-1));

            return declaration;
        }

        private Declaration CreateTestingDeclaration()
        {
            return new Declaration()
            {
                TaxDeclaration = new TaxDeclaration()
                {
                    Taxes = new Taxes()
                    {
                        UnknownConstantField2 = "Z",
                        Year = 2019,//
                        TaxPeriodStart = "01.01.2019",//
                        TaxPeriodEnds = "31.12.2019",//
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
                        Row58 = "0.00",
                        Row60 = "0",
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
                        TaxYearPhone = "21",
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
                        Row40 = "0",
                        Row41 = "96",
                        Row41a = "96",
                        Row42 = "126",
                        Row43 = "38",
                        Row45 = "126"
                    },
                    Section3 = new Section3()
                    {
                        Row56 = "0",
                        Row49 = "44",
                        Row50 = "45",
                        Row54 = "52",
                        Row51 = "46",
                        Row46 = "41",
                        Row48 = "43",
                        Row47 = "42",
                        Row53MonthsCell = "50",
                        Row55 = "74",
                        Row53ValueCell = "51",
                        Row53DescriptionCell = "49",
                        Row57 = "0",
                        Row52 = "47",
                        Row52a = "48"
                    },
                    Appendix2OtherIncomeRow = new Appendix2OtherIncomeRow[]
                    {
                        new Appendix2OtherIncomeRow() { Code = "Z", Type = "D", Description = "94", Expenses = "96", Income = "95", Profit = "-1" }
                    },
                    AppendixIncomeTable = new AppendixIncomeTableRow[]
                    {
                        new AppendixIncomeTableRow() { Income = 101, Insurance = "102", TakenDeposit = "103", TakenTaxByArticle36Paragraph8 = "106", TakenTaxByArticle36Paragraph7 = "105", PaidMontlyBonuses = "104" },
                        new AppendixIncomeTableRow() { Income = 107, Insurance = "108", TakenDeposit = "109", TakenTaxByArticle36Paragraph8 = "112", TakenTaxByArticle36Paragraph7 = "111", PaidMontlyBonuses = "110" },
                    },
                    Appendix3Lists = new Appendix3List[]
                    {
                        new Appendix3List() { Coeficient = 0.00M, Country = "US", Income = 97, MaximumRecognizedTax = 0.00M, RecognizedTax = 0.00M, Tax = 99, UnrecognisedTax = 99.00M, Expenses = 98M }
                    },
                    AppendixSeznam = new AppendixSeznamRow[]
                    {
                        new AppendixSeznamRow() { BrokerName = "107", Country = "AZ", IncomeInCZK = 110, TaxInCZK = 109, TaxInOriginalCurrency = 108 },
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
                    },
                    Appendix3 = new Appendix3()
                    {
                        Row328 = 0.00M,
                        Row329 = 99.00M,
                        Row330 = 0.00M
                    },
                    Appendix2 = new Appendix2()
                    {
                        Row207 = "95",
                        Row208 = "95",
                        Row209 = "0",
                        TotalExpenses = "96",
                        TotalIncomes = "95",
                        TotalProfit = "0",
                        ApplyForExpensesGivenByFixedPercentage = "N",
                        IncomesFromPartnerSharedWealth = "N"
                    },
                    Appendix1 = new Appendix1()
                    {
                        ApplyForExpensesGivenByFixedPercentage = "N"
                    }
                }
            };

        }
    }
}
