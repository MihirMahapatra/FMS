using FMS.Db.DbEntity;

namespace FMS.Model.CommonModel
{
    public class PurchaseTransactionModel
    {
        public Guid PurchaseId { get; set; }
        public Guid Fk_PurchaseOrderId { get; set; }
        public string TransactionNo { get; set; }
        public DateTime? TransactionDate { get; set; }
        public Guid Fk_ProductId { get; set; }
        public string ProductName { get; set; }
        public Guid Fk_BranchId { get; set; }
        public Guid Fk_FinancialYearId { get; set; }
        public decimal AlternateQuantity { get; set; }
        public Guid Fk_AlternateUnitId { get; set; }
        public decimal UnitQuantity { get; set; }
        public string UnitName {  get; set; }
        public decimal Rate { get; set; }
        public decimal Discount { get; set; }
        public decimal GstAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Gst { get; set; }
        public decimal Amount { get; set; }
        public BranchModel Branch { get; set; }
        public FinancialYearModel FinancialYear { get; set; }
        public PurchaseOrderModel PurchaseOrder { get; set; } 
        public ProductModel Product { get; set; }
        public AlternateUnitModel AlternateUnit { get; set; }
    }
}
