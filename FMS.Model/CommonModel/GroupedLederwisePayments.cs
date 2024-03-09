using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.CommonModel
{
    public class GroupedLederwisePayments
    {
        public string VoucherNo { get; set; }
        public List<LederwisePayments> LederwisePayments { get; set; }
    }
    public class LederwisePayments
    {
        public Guid Fk_LedgerId { get; set; }
        public List<PaymentModel> Payments { get; set; }
    }
}
