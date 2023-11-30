namespace FMS.Model.CommonModel
{
    public class LedgerGroupModel : Base
    {
        public Guid LedgerGroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupAlias { get; set; }
        public List<LedgerSubGroupModel> LedgerSubGroups { get; set; }
        public List<LedgerModel> Ledgers { get; set; } 
        public List<JournalModel> Journals { get; set; } 
        public List<PaymentModel> Payments { get; set; } 
        public List<ReceiptModel> Receipts { get; set; }
    }
}
