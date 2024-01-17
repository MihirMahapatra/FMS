namespace FMS.Db.DbEntity
{
    public class SubLedger
    {
        public Guid SubLedgerId { get; set; }
        public Guid Fk_LedgerId { get; set; }
        public Guid? Fk_BranchId { get; set; }
        public string SubLedgerName { get; set; }
        public Ledger Ledger { get; set; }
        public LedgerDev LedgerDev { get; set; }
        public Branch Branch { get; set; }
        public ICollection<SubLedgerBalance> SubLedgerBalances { get; set; }
        public ICollection<Labour> Labours { get; set; }
        public ICollection<Party> Parties { get; set; }
        public ICollection<Journal> Journals { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<Receipt> Receipts { get; set; }
        public ICollection<SalesOrder> SalesOrders { get; set; }
        public ICollection<PurchaseOrder> PurchaseOrders { get; set; }
        public ICollection<SalesReturnOrder> SalesReturnOrders { get; set; }
        public ICollection<PurchaseReturnOrder> PurchaseReturnOrders { get; set; }
    }
}
