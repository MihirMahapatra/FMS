namespace FMS.Model.CommonModel
{
    public class ProductionEntryModel : Base
    {
        public Guid ProductionEntryId { get; set; }
        public string ProductionNo { get; set; }
        public string Date { get; set; }
        public DateTime ProductionDate { get; set; }
        public Guid Fk_ProductId { get; set; }
        public Guid Fk_LabourId { get; set; }
        public string LabourType { get; set; }
        public Guid Fk_FinancialYearId { get; set; }
        public Guid FK_BranchId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public ProductModel Product { get; set; } 
        public LabourModel Labour { get; set; }
        public FinancialYearModel FinancialYear { get; set; } 
        public BranchModel Branch { get; set; }
        public List<ProductionEntryTransactionModel> ProductionEntryTransactions { get; set; }
    }
}
  