
namespace FMS.Model.CommonModel
{
    public class LedgerBalanceModel
    {
        public Guid LedgerBalanceId { get; set; }
        public Guid Fk_LedgerId { get; set; }
        public Guid Fk_BranchId { get; set; }
        public Guid Fk_FinancialYear { get; set; }
        public decimal OpeningBalance { get; set; }
        public string OpeningBalanceType { get; set; }
        public decimal RunningBalance { get; set; }
        public string RunningBalanceType { get; set; }
        public LedgerModel Ledger { get; set; }
        public BranchModel Branch { get; set; }
        public FinancialYearModel FinancialYear { get; set; }
        public List<SubLedgerBalanceModel> SubLedgerBalances { get; set; }
        
    }
}
