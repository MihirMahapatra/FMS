using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.CommonModel
{
    public class StockReportDataModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ProductName { get; set; }
        public List<StockReportSummerizedModel> StockReport { get; set; }
        public ProductModel Product { get; set; }
    }
}
