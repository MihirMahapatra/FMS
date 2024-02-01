using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class StockReportSummerizedViewModel : Base
    {
        public StockReportSummerizedViewModel()
        {
            StockReports = new List<StockReportSummerizedModel>();
        }
        public List<StockReportSummerizedModel> StockReports { get; set; }
    }
}
