using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.CommonModel
{
    public class LedgerbookDataRequest
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public Guid LedgerId { get; set; }
    }
}
