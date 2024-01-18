namespace FMS.Model.CommonModel
{
    public class LedgerModel : Base
    {
        public Guid LedgerId { get; set; }
        public string LedgerName { get; set; }
        public string LedgerType { get; set; }
        public string HasSubLedger { get; set; }
        public Guid Fk_LedgerGroupId { get; set; }
        public Guid? Fk_LedgerSubGroupId { get; set; }
        public LedgerGroupModel LedgerGroup { get; set; } 
        public LedgerSubGroupModel LedgerSubGroup { get; set; }
        public List<SubLedgerModel> SubLedgers { get; set; }
        public List<LedgerBalanceModel> LedgerBalances { get; set; }
        public List<PartyModel> Parties { get; set; }
        public List<JournalModel> Journals { get; set; }
        public List<PaymentModel> Payments { get; set; }
        public List<ReceiptModel> Receipts { get; set; }

    }
}
