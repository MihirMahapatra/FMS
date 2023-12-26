using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.CommonModel
{
    public class LedgerTrialBalanceModel
    {
        public string LedgerId { get; set; }
        public string LedgerName { get; set; }
        public decimal DebitTotal { get; set; }
        public decimal CreditTotal { get; set; }
        public decimal OpeningBal { get; set; }
        public decimal ClosingBal { get;set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

    }
}
