namespace FMS.Model.CommonModel
{
    public class BranchFinancialYearModel
    {
        public Guid BranchFinancialYearId { get; set; }
        public Guid Fk_FinancialYearId { get; set; }
        public Guid FK_BranchId { get; set; }
        public BranchModel Branch { get; set; }
        public FinancialYearModel FinancialYear { get; set; }
    }
}
