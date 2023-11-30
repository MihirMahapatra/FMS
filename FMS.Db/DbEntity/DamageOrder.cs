namespace FMS.Db.DbEntity
{
    public class DamageOrder
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
        public Branch Branch { get; set; }
        public FinancialYear FinancialYear { get; set; }
        public ProductType ProductType { get; set; }
        public Labour Labour { get; set; }
        public ICollection<DamageTransaction> DamageTransactions { get; set; }
    }
}
