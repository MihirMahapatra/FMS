using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class LaborReportViewModel : Base
    {
        public LaborReportViewModel()
        {
            LaborReports = new List<LaborReportModel>();
        }
        public List<LaborReportModel> LaborReports { get; set; }
    }
}
