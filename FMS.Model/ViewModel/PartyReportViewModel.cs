using FMS.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.ViewModel
{
    public class PartyReportViewModel:Base
    {
        public PartyReportViewModel()
        {
            PartyReports = new List<PartyReportModel>();
        }
        public List<PartyReportModel> PartyReports { get; set; }
    }
}
