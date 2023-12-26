using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.CommonModel
{
    public class LedgerBookModel
    {
        public string VouvherNo { get; set; }
        public string VoucherDate { get; set; }
        public string FromDate { get; set; }
        public string Todate { get; set; }
        public string PartyName { get; set; }
        public decimal OpeningBal { get; set; }
        public decimal ClosingBal { get; set; }
        public string OpeningBalType { get; set; }
        public string BalanceType { get; set; }
        public string ChequeNo { get; set; }
        public string ChequeDate { get; set; }
        public string CashBank { get; set; }
        public Guid? CashBankLedgerId { get; set; }
        public Guid Fk_LedgerGroupId { get; set; }
        public Guid Fk_LedgerId { get; set; }
        public Guid? Fk_SubLedgerId { get; set; }
        public Guid Fk_BranchId { get; set; }
        public Guid Fk_FinancialYearId { get; set; }
        public string TransactionNo { get; set; }
        public string narration { get; set; }
        public decimal Amount { get; set; }
        public string DrCr { get; set; }
        public LedgerGroupModel LedgerGroup { get; set; }
        public LedgerModel Ledger { get; set; }
        public SubLedgerModel SubLedger { get; set; }
        public BranchModel Branch { get; set; }
        public FinancialYearModel FinancialYear { get; set; }
        public string FromAcc { get; set; }
        public string ToAcc { get; set; }
        public string LedgerDevName { get; set; }
        public string LedgerName { get; set; }
        public string SubLedgerName { get; set; }
        public List<ReceiptModel> Receipts { get; set; }
        public List<PaymentModel> Payments { get; set; }
        public List<JournalModel> Journals { get; set; }
    }
}
