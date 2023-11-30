﻿namespace FMS.Db.DbEntity
{
    public class LabourRate
    {
        public Guid LabourRateId { get; set; }
        public Guid? Fk_BranchId {  get; set; }
        public Guid? Fk_FinancialYearId {  get; set; }
        public DateTime? Date { get; set; }
        public Guid Fk_ProductId { get; set; }
        public int Rate { get; set; }
        public Product Product { get; set; }
        public Branch Branch { get; set; }
        public FinancialYear FinancialYear { get; set; }
    }
}
