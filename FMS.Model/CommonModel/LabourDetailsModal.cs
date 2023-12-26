using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.CommonModel
{
    public class LabourDetailsModal
    {
        public string LabourName { get; set; }
        public decimal OpeningBal { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<LaborReportModel> LaborReports { get; set; }    
    }
}
