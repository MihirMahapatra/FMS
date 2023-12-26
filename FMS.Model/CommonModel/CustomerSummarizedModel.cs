using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.CommonModel
{
    public class CustomerSummarizedModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string PartyName { get; set; }
        public List<PartyReportModel> PartyReports { get; set; }
        public List<PartyModel> Party { get; set; }
    }
}
