namespace FMS.Db.DbEntity
{
    public class SubLedgerBalance
    {
        public Guid SubLedgerBalanceId { get; set; }
        public Guid Fk_LedgerBalanceId { get; set; }
        public Guid Fk_SubLedgerId { get; set; }
        public Guid? Fk_BranchId { get; set; }
        public Guid Fk_FinancialYearId { get; set; }
        public decimal OpeningBalance { get; set; }
        public string OpeningBalanceType { get; set; }
        public decimal RunningBalance { get; set; }
        public string RunningBalanceType { get; set; }
        public SubLedger SubLedger { get; set; }    
        public Branch Branch { get; set; }
        public FinancialYear FinancialYear { get; set; }
        public LedgerBalance LedgerBalance { get; set; }
    }
}
