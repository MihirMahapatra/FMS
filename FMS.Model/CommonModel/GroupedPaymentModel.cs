using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.CommonModel
{
    public class GroupedPaymentModel
    {
        public string VoucherNo { get; set; }
        public List<PaymentModel> Payments { get; set; }
    }
}
