using FMS.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.ViewModel
{
    public class DaySheetViewModel : Base
    {
        public DaySheetViewModel()
        {
            DaySheet = new DaySheetModel();
        }
        public DaySheetModel DaySheet { get; set; }
    }
}
