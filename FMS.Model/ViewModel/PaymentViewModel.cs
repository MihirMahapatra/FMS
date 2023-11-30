using FMS.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.ViewModel
{
    public class PaymentViewModel : Base
    {
        public PaymentViewModel()
        {
            Payment = new PaymentModel();
            Payments = new List<PaymentModel>();
        }
        public List<PaymentModel> Payments { get; set; }
        public PaymentModel Payment { get; set; }
        public List<GroupedPaymentModel> GroupedPayments { get; set; }
    }
}
