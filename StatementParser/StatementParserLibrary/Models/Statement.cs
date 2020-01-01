using System;
using System.Collections.Generic;

namespace StatementParserLibrary.Models
{
    public class Statement
    {
        public Statement(IList<Transaction> transactions)
        {
            this.Transactions = transactions ?? throw new ArgumentNullException(nameof(transactions));
        }

        public IList<Transaction> Transactions { get; }

        public override string ToString()
        {
            return String.Join("\r\n", Transactions);
        }
    }
}
