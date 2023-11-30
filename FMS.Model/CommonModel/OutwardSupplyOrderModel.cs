namespace FMS.Model.CommonModel
{
    public class OutwardSupplyOrderModel
    {
        public Guid OutwardSupplyOrderId { get; set; }
        public string TransactionNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public Guid ToBranch { get; set; }
        public string BranchName { get; set; }
        public Guid Fk_ProductTypeId { get; set; }
        public Guid Fk_BranchId { get; set; }
        public Guid Fk_FinancialYearId { get; set; }
        public decimal TotalAmount { get; set; }
        public BranchModel Branch { get; set; }
        public FinancialYearModel FinancialYear { get; set; }
        public ProductTypeModel ProductType { get; set; }
        public List<OutwardSupplyTransactionModel> OutwardSupplyTransactions { get; set; }
        public string ProductTypeName { get; set; }
    }
}
