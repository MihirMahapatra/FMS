namespace FMS.Model.CommonModel
{
    public class InwardSupplyTransactionModel
    {
        public Guid InwardSupplyTransactionId { get; set; }
        public Guid Fk_InwardSupplyOrderId { get; set; }
        public string TransactionNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public Guid Fk_ProductId { get; set; }
        public Guid Fk_BranchId { get; set; }
        public Guid Fk_FinancialYearId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public InwardSupplyOrderModel InwardSupplyOrder { get; set; }
        public ProductModel Product { get; set; }
        public BranchModel Branch { get; set; }
        public FinancialYearModel FinancialYear { get; set; }
    }
}
