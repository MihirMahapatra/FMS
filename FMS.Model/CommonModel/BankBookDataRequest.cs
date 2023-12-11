using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.CommonModel
{
    public class BankBookDataRequest
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string BankId { get; set; }
    }
}
