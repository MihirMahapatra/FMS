namespace FMS.Model.CommonModel
{
    public class LabourRateModel : Base
    {
        public Guid LabourRateId { get; set; }
        public Guid Fk_BranchId { get; set; }
        public Guid Fk_FinancialYearId { get; set; }
        public string FormtedDate { get; set; }
        public DateTime? Date { get; set; }
        public Guid Fk_ProductId { get; set; }
        public decimal Rate { get; set; }
        public ProductModel Product { get; set; } 
        public BranchModel Branch { get; set; } 
        public FinancialYearModel FinancialYear { get; set; } 
    }
}
