using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.CommonModel
{
    public class TransactionModel
    {
        public string VouvherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public string DrCr { get; set; }
        public string narration { get; set; }
        public decimal Amount { get; set; }
    }
}
