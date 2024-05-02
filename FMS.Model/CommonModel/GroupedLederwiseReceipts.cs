using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.CommonModel
{
    public class GroupedLederwiseReceipts
    {
        public string VoucherNo { get; set; }
        public List<LederwiseReceipts> LederwiseReceipts { get; set; }
    }
    public class LederwiseReceipts
    {
        public Guid Fk_LedgerId { get; set; }
        public List<ReceiptModel> Receipts { get; set; }
    }
}
