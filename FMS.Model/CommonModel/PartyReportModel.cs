using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.CommonModel
{
    public class PartyReportModel
    {
        public string PartyName { get; set; }
        public decimal OpeningBal { get; set; }
        public string OpeningBalType { get; set; }
        public decimal DrAmt { get; set; }
        public decimal CrAmt { get; set; }
        public decimal Balance { get; set; }
        public string BalanceType { get; set; }
    }
}
