namespace FMS.Model.CommonModel
{
    public class DamageTransactionModel
    {
        public Guid DamageTransactionId { get; set; }
        public Guid Fk_DamageOrderId { get; set; }
        public string TransactionNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public Guid Fk_ProductId { get; set; }
        public Guid Fk_BranchId { get; set; }
        public Guid Fk_FinancialYearId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public DamageOrderModel DamageOrder { get; set; }
        public ProductModel Product { get; set; }
        public BranchModel Branch { get; set; }
        public BranchFinancialYearModel FinancialYear { get; set; }
    }
}
