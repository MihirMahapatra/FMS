namespace FMS.Db.DbEntity
{
    public class LedgerBalance
    {
        public Guid LedgerBalanceId { get; set; }
        public Guid Fk_LedgerId { get; set; }
        public Guid? Fk_BranchId { get; set; }
        public Guid Fk_FinancialYear { get; set; }
        public decimal OpeningBalance { get; set; }
        public string OpeningBalanceType { get; set; }
        public decimal RunningBalance { get; set; }
        public string RunningBalanceType { get; set; }
        public Ledger Ledger { get; set; }
        public LedgerDev LedgerDev { get; set; }
        public Branch Branch { get; set; }
        public FinancialYear FinancialYear { get; set; }
        public ICollection<SubLedgerBalance> SubLedgerBalances { get; set; }
    }
}
