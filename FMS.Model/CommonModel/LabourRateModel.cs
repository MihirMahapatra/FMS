using FMS.Db.DbEntity;

namespace FMS.Model.CommonModel
{
    public class LabourRateModel : Base
    {
        public Guid LabourRateId { get; set; }
        public string FinancialYear { get; set; } = null;
        public string FormtedDate { get; set; }
        public DateTime? Date { get; set; }
        public Guid? Fk_ProductTypeId { get; set; }
        public Guid Fk_ProductId { get; set; }
        public Guid? Fk_BranchId { get; set; }
        public decimal Rate { get; set; }
        public ProductTypeModel ProductType { get; set; }
        public ProductModel Product { get; set; }
        public BranchModel Branch { get; set; }
    }
}
