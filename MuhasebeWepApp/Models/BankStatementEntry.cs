using System;

namespace MuhasebeWepApp.Models
{
    public class BankStatementEntry
    {
        public DateTime DocumentDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Debt { get; set; }
        public decimal Credit { get; set; }
        public string ClosingAccount { get; set; } = string.Empty;
    }
}
