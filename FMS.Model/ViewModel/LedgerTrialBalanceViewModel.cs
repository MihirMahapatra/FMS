using FMS.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Model.ViewModel
{
    public class LedgerTrialBalanceViewModel : Base
    {
        public LedgerTrialBalanceViewModel()
        {
            TrialBalance = new LedgerTrialBalanceModel();
            TrialBalances = new List<LedgerTrialBalanceModel>();
        }
        public List<LedgerTrialBalanceModel> TrialBalances { get; set; }
        public LedgerTrialBalanceModel TrialBalance { get; set; }
    }
}
