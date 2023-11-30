using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class FinancialYearViewModel : Base
    {
        public FinancialYearViewModel()
        {
            FinancialYears = new List<FinancialYearModel>();
            FinancialYear = new FinancialYearModel();
        }
        public List<FinancialYearModel> FinancialYears { get; set; }
        public FinancialYearModel FinancialYear { get; set; }
    }
}
