using System;
using System.Collections.Generic;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using StatementParserLibrary.Models;

namespace StatementParserLibrary.Parsers
{
    internal class MorganStanleyStatementParser2018 : IStatementParser
    {
        private const int HeaderRowOffset = 8;
        private const int FooterRowOffset = 6;
        private const string SheetName = "4_Stock Award Plan_Completed";
        private const string Signature = "Morgan Stanley Smith Barney LLC. Member SIPC.";

        private Dictionary<string, TransactionType> transactionTypeMapping = new Dictionary<string, TransactionType> {
            { "Dividend Reinvested", TransactionType.DividendReinvestment },
            { "IRS Withholding", TransactionType.IRSWitholding },
            { "Dividend Credit", TransactionType.DividendCredit },
            { "Share Deposit", TransactionType.ShareDeposit },
            { "Sale", TransactionType.Sell },
            { "Wire Transfer", TransactionType.WireTransfer }
        };

        public bool CanParse(string statementFilePath)
        {
            if (!statementFilePath.EndsWith(".xls", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            var sheet = GetSheet(statementFilePath);

            return sheet.GetRow(sheet.LastRowNum).Cells[0].StringCellValue == Signature;
        }

        public Statement Parse(string statementFilePath)
        {
            var transactions = new List<Transaction>();

            var sheet = GetSheet(statementFilePath);

            var name = sheet.GetRow(3).Cells[1].StringCellValue;

            for (int i = HeaderRowOffset; i <= sheet.LastRowNum - FooterRowOffset; i++)
            {
                var row = sheet.GetRow(i);

                var date = DateTime.Parse(row.Cells[0].StringCellValue);
                var type = this.ResolveTransactionType(row.Cells[1].StringCellValue);
                var amount = Convert.ToDecimal(row.Cells[3].NumericCellValue);
                var price = Convert.ToDecimal(row.Cells[4].NumericCellValue);
                var grossProceeds = Convert.ToDecimal(row.Cells[5].NumericCellValue);

                transactions.Add(new Transaction(Broker.MorganStanley, date, name, type, amount, price, grossProceeds, Currency.USD));
            }

            return new Statement(transactions);
        }

        private ISheet GetSheet(string statementFilePath)
        {
            HSSFWorkbook workbook = null;
            using (FileStream stream = new FileStream(statementFilePath, FileMode.Open, FileAccess.Read))
            {
                workbook = new HSSFWorkbook(stream);
            }

            return workbook.GetSheet(SheetName);
        }

        private TransactionType ResolveTransactionType(string type)
        {
            if (transactionTypeMapping.TryGetValue(type, out var result))
            {
                return result;
            }

            throw new NotSupportedException($"Transaction type: {type} is not supported");
        }
    }
}
