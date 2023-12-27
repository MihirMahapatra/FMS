namespace FMS.Db.DbEntity
{
    public class FinancialYear
    {
        public Guid FinancialYearId { get; set; }
        public Guid FK_BranchId { get; set; }
        public String Financial_Year { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Branch Branch { get; set; }
        public ICollection<Stock> Stocks { get; set; }
        public ICollection<LabourRate> LabourRates { get; set; }
        public ICollection<ProductionEntry> ProductionEntries { get; set; }
        public ICollection<ProductionEntryTransaction> ProductionEntryTransactions { get; set; }
        public ICollection<LedgerBalance> LedgerBalances { get; set; }
        public ICollection<SubLedgerBalance> SubLedgerBalances { get; set; }
        public ICollection<Journal> Journals { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<Receipt> Receipts { get; set; }
        public ICollection<PurchaseOrder> PurchaseOrders { get; set; }
        public ICollection<SalesOrder> SalesOrders { get; set; }
        public ICollection<PurchaseTransaction> PurchaseTransactions { get; set; }
        public ICollection<SalesTransaction> SalesTransactions { get; set; }
        public ICollection<SalesReturnOrder> SalesReturnOrders { get; set; }
        public ICollection<PurchaseReturnOrder> PurchaseReturnOrders { get; set; }
        public ICollection<SalesReturnTransaction> SalesReturnTransactions { get; set; }
        public ICollection<PurchaseReturnTransaction> PurchaseReturnTransactions { get; set; }
        public ICollection<InwardSupplyOrder> InwardSupplyOrders { get; set; }
        public ICollection<OutwardSupplyOrder> OutwardSupplyOrders { get; set; }
        public ICollection<InwardSupplyTransaction> InwardSupplyTransactions { get; set; }
        public ICollection<OutwardSupplyTransaction> OutwardSupplyTransactions { get; set; }
        public ICollection<DamageOrder> DamageOrders { get; set; }
        public ICollection<DamageTransaction> DamageTransactions { get; set; }
    }
}
