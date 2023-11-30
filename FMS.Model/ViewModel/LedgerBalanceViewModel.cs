using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class LedgerBalanceViewModel : Base
    {
        public LedgerBalanceViewModel()
        {
            LedgerBalances = new List<LedgerBalanceModel>();
            LedgerBalance = new LedgerBalanceModel();
        }
        public List<LedgerBalanceModel> LedgerBalances { get; set; }
        public LedgerBalanceModel LedgerBalance { get; set; }
    }
}
