namespace FMS.Model.CommonModel
{
    public class PaymentModel
    {
        public Guid PaymentId { get; set; }
        public string VouvherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public string ChequeNo { get; set; } = null;
        public DateTime? ChequeDate { get; set; }
        public string CashBank { get; set; }
        public Guid? CashBankLedgerId { get; set; }
        public Guid Fk_LedgerGroupId { get; set; }
        public Guid Fk_LedgerId { get; set; }
        public Guid? Fk_SubLedgerId { get; set; }
        public Guid Fk_BranchId { get; set; }
        public Guid Fk_FinancialYearId { get; set; }
        public string Narration { get; set; }
        public decimal Amount { get; set; }
        public string DrCr { get; set; }
        public LedgerGroupModel LedgerGroup { get; set; } 
        public LedgerModel Ledger { get; set; }
        public SubLedgerModel SubLedger { get; set; } 
        public BranchModel Branch { get; set; }
        public FinancialYearModel FinancialYear { get; set; }
        public string ToAcc { get; set; }
        public string LedgerDevName { get; set; }        
        public string LedgerName { get; set; }
        public string SubLedgerName { get; set; }
    }
}
