namespace FMS.Db.DbEntity
{
    public class InwardSupplyOrder
    {
        public Guid InwardSupplyOrderId { get; set; }
        public string TransactionNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public Guid FromBranch { get; set; }
        public Guid Fk_ProductTypeId { get; set; }
        public Guid Fk_BranchId { get; set; }
        public Guid Fk_FinancialYearId { get; set; }
        public decimal TotalAmount { get; set; }
        public Branch Branch { get; set; }
        public FinancialYear FinancialYear { get; set; }
        public ProductType ProductType { get; set; }
        public ICollection<InwardSupplyTransaction> InwardSupplyTransactions { get; set; }
    }
}
