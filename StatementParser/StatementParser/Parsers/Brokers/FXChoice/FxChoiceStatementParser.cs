using System;
using System.Collections.Generic;
using System.IO;
using StatementParser.Models;

using PigPdfDocument = UglyToad.PdfPig.PdfDocument;

namespace StatementParser.Parsers.Brokers.FxChoice
{
    public class FxChoiceStatementParser : ITransactionParser
    {
        public bool CanParse(string statementFilePath)
        {
            if (!File.Exists(statementFilePath) || Path.GetExtension(statementFilePath).ToLowerInvariant() != ".pdf")
            {
                return false;
            }

            return true;
        }

        public IList<Transaction> Parse(string statementFilePath)
        {
            using (var document = PigPdfDocument.Open(statementFilePath))
            {


                return new List<Transaction>();
            }
        }
    }
}
