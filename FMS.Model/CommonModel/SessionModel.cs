using System.ComponentModel.DataAnnotations;

namespace FMS.Model.CommonModel
{
    public class SessionModel : Base
    {
        [Required]
        public string FinancialYearId { get; set; }
        [Required]
        public string BranchId { get; set; }
        public List<BranchModel> Branches { get; set; }
        public List<BranchFinancialYearModel> FinancialYears { get; set; }
    }
}
