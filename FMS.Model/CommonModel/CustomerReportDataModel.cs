using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.CommonModel
{
    public class CustomerReportDataModel
    {
        public CompanyDetailsModel Cmopany { get; set; }
        public CustomerSummarizedModel Customers { get; set; }
    }
}
