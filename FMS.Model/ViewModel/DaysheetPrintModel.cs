using FMS.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.ViewModel
{
    public class DaysheetPrintModel
    {
        public CompanyModel Cmopany { get; set; }
        public DaySheetModel daySheet { get; set; }
    }
}
