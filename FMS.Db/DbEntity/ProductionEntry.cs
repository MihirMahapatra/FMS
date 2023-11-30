namespace FMS.Db.DbEntity
{
    public class ProductionEntry
    {
        public Guid ProductionEntryId { get; set; }
        public string ProductionNo { get; set; }
        public DateTime ProductionDate { get; set; }
        public Guid Fk_ProductId { get; set; }
        public Guid Fk_LabourId { get; set; }
        public string LabourType { get; set; }
        public Guid Fk_FinancialYearId { get; set; }
        public Guid FK_BranchId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public Product Product { get; set; }
        public Labour Labour { get; set; }
        public FinancialYear FinancialYear { get; set; }
        public Branch Branch { get; set; }
        public ICollection<ProductionEntryTransaction> ProductionEntryTransactions { get; set; }
    }
}
