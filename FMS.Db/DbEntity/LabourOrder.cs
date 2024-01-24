namespace FMS.Db.DbEntity
{
    public class LabourOrder
    {
        public Guid LabourOrderId { get; set; }
        public string TransactionNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public Guid Fk_ProductId { get; set; }
        public Guid Fk_LabourId { get; set; }
        public Guid Fk_LabourTypeId { get; set; }
        public Guid Fk_FinancialYearId { get; set; }
        public Guid FK_BranchId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public decimal OTAmount { get; set; }
        public Product Product { get; set; }
        public Labour Labour { get; set; }
        public FinancialYear FinancialYear { get; set; }
        public Branch Branch { get; set; }
        public LabourType LabourType { get; set; }
        public ICollection<LabourTransaction> LabourTransactions { get; set; }
    }
}
