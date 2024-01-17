namespace FMS.Db.DbEntity
{
    public class LabourTransaction
    {
        public Guid LabourTransactionId { get; set; }
        public string TransactionNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public Guid Fk_ProductionEntryId { get; set; }
        public Guid Fk_ProductId { get; set; }
        public Guid Fk_BranchId { get; set; }
        public Guid Fk_FinancialYearId { get; set; }
        public decimal Quantity { get; set; }
        public LabourOrder LabourOrder { get; set; }
        public Product Product { get; set; }
        public FinancialYear FinancialYear { get; set; }
        public Branch Branch { get; set; }
      
    }
}
