namespace FMS.Model.CommonModel
{
    public class SubLedgerModel : Base
    {
        public Guid SubLedgerId { get; set; }
        public Guid Fk_LedgerId { get; set; }
        public Guid? Fk_BranchId { get; set; }
        public string SubLedgerName { get; set; }
        public LedgerModel Ledger { get; set; }
        public List<SubLedgerBalanceModel> SubLedgerBalances { get; set; }
        public List<LabourModel> Labours { get; set; }
        public List<PartyModel> Parties { get; set; }
        public List<JournalModel> Journals { get; set; }
        public List<PaymentModel> Payments { get; set; }
        public List<ReceiptModel> Receipts { get; set; }
        public List<SalesOrderModel> SalesOrders { get; set; }
        public List<PurchaseOrderModel> PurchaseOrders { get; set; }
        public List<SalesReturnOrderModel> SalesReturnOrders { get; set; }
        public List<PurchaseReturnOrderModel> PurchaseReturnOrders { get; set; }

        //others
        public string LedgerName { get; set; }
        public string Adress { get; set; }
    }
}
