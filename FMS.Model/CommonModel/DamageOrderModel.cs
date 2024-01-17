namespace FMS.Model.CommonModel
{
    public class DamageOrderModel
    {
        public Guid DamageOrderId { get; set; }
        public string TransactionNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public Guid Fk_ProductTypeId { get; set; }
        public Guid? Fk_LabourId { get; set; }
        public Guid Fk_BranchId { get; set; }
        public Guid Fk_FinancialYearId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Reason { get; set; } = null;
        public BranchModel Branch { get; set; }
        public BranchFinancialYearModel FinancialYear { get; set; }
        public ProductTypeModel ProductType { get; set; }
        public LabourModel Labour { get; set; }
        public List<DamageTransactionModel> DamageTransactions { get; set; }
        public string ProductTypeName { get; set; }
    }
}
