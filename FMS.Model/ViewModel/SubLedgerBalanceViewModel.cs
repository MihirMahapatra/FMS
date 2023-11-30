using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class SubLedgerBalanceViewModel : Base
    {
        public SubLedgerBalanceViewModel()
        {
            SubLedgerBalances = new List<SubLedgerBalanceModel>();
            SubLedgerBalance = new SubLedgerBalanceModel();
        }
        public List<SubLedgerBalanceModel> SubLedgerBalances { get; set; }
        public SubLedgerBalanceModel SubLedgerBalance { get; set; }
    }
}
