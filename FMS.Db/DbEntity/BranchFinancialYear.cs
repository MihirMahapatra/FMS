namespace FMS.Db.DbEntity
{
    public class BranchFinancialYear
    {
        public Guid BranchFinancialYearId { get; set; }
        public Guid Fk_FinancialYearId { get; set; }
        public Guid Fk_BranchId { get; set; }
        public Branch Branch { get; set; }
        public FinancialYear FinancialYear { get; set; }
        
    }
}
