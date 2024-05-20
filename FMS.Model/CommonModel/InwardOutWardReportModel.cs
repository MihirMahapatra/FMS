using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.CommonModel
{
    public class InwardOutWardReportModel
    {
        public List<InwardOutWardTransationModel> Orders { get; set; } = new List<InwardOutWardTransationModel>();
    }
    public class InwardOutWardTransationModel
    {
        public string Product { get; set; }
        public decimal Quantity { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionNo { get; set; }
        public string BranchName { get; set; }
        public string VoucherType { get; set; }
    }

    public class InwardOutWardReportViewModel : Base
    {
        public InwardOutWardReportViewModel()
        {
            InwardOutwardTransation = new InwardOutWardReportModel();
            InwardOutwardTransations = new List<InwardOutWardReportModel>();
        }
        public List<InwardOutWardReportModel> InwardOutwardTransations { get; set; }
        public InwardOutWardReportModel InwardOutwardTransation { get; set; }
    }

}
