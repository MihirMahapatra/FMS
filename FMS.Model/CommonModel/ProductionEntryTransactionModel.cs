namespace FMS.Model.CommonModel
{
    public class ProductionEntryTransactionModel
    {
        public Guid ProductionEntryTransactionId { get; set; }
        public Guid Fk_ProductionEntryId { get; set; }
        public Guid Fk_ProductId { get; set; }
        public Guid Fk_BranchId { get; set; }
        public Guid Fk_FinancialYearId { get; set; }
        public decimal Quantity { get; set; }
        public string TransactionNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public ProductionEntryModel ProductionEntry { get; set; }
        public ProductModel Product { get; set; }
        public FinancialYearModel FinancialYear { get; set; }
        public BranchModel Branch { get; set; }
    }
}
