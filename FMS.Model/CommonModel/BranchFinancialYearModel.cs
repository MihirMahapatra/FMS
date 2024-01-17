namespace FMS.Model.CommonModel
{
    public class BranchFinancialYearModel : Base
    {
        public Guid BranchFinancialYearId { get; set; }
        public Guid Fk_FinancialYearId { get; set; }
        public Guid FK_BranchId { get; set; }
        public BranchModel Branch { get; set; }  
        public FinancialYearModel FinancialYear{ get; set; }
        public List<StockModel> Stocks { get; set; } 
        public List<ProductionEntryModel> ProductionEntries { get; set; }
        public List<ProductionEntryTransactionModel> ProductionEntryTransactions { get; set; }
        public List<LedgerBalanceModel> LedgerBalances { get; set; }
        public List<SubLedgerBalanceModel> SubLedgerBalances { get; set; }
        public List<JournalModel> Journals { get; set; } 
        public List<PaymentModel> Payments { get; set; } 
        public List<ReceiptModel> Receipts { get; set; } 
        public List<PurchaseOrderModel> PurchaseOrders { get; set; }
        public List<SalesOrderModel> SalesOrders { get; set; }
        public List<PurchaseTransactionModel> PurchaseTransactions { get; set; }
        public List<SalesTransactionModel> SalesTransactions { get; set; }
        public List<SalesReturnOrderModel> SalesReturnOrders { get; set; }
        public List<PurchaseReturnOrderModel> PurchaseReturnOrders { get; set; }
        public List<SalesReturnTransactionModel> SalesReturnTransactions { get; set; } 
        public List<PurchaseReturnTransactionModel> PurchaseReturnTransactions { get; set; }
        public List<InwardSupplyOrderModel> InwardSupplyOrders { get; set; }
        public List<OutwardSupplyOrderModel> OutwardSupplyOrders { get; set; }
        public List<InwardSupplyTransactionModel> InwardSupplyTransactions { get; set; }
        public List<OutwardSupplyTransactionModel> OutwardSupplyTransactions { get; set; }
        public List<DamageOrderModel> DamageOrders { get; set; }
        public List<DamageTransactionModel> DamageTransactions { get; set; }
    }
}
