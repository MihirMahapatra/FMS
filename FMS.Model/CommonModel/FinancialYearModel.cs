using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.CommonModel
{
    public class FinancialYearModel
    {
        public Guid FinancialYearId { get; set; }
        public string Financial_Year { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
