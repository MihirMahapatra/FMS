using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class StockReportViewModel : Base
    {
        public StockReportViewModel()
        {
            StockReports = new List<StockReportModel>();
        }
        public List<StockReportModel> StockReports { get; set; }
    }
}
