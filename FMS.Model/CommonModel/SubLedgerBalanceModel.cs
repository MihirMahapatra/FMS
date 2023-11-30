namespace FMS.Model.CommonModel
{
    public class SubLedgerBalanceModel
    {
        public Guid SubLedgerBalanceId { get; set; }
        public Guid Fk_SubLedgerId { get; set; }
        public Guid Fk_BranchId { get; set; }
        public Guid Fk_FinancialYearId { get; set; }
        public decimal OpeningBalance { get; set; }
        public string OpeningBalanceType { get; set; }
        public decimal RunningBalance { get; set; }
        public string RunningBalanceType { get; set; }
        public SubLedgerModel SubLedger { get; set; } 
        public BranchModel Branch { get; set; } 
        public FinancialYearModel FinancialYear { get; set; }
        public LedgerBalanceModel LedgerBalance { get; set; }
    }
}
