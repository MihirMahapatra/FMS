using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class LedgerViewModel : Base
    {
        public LedgerViewModel()
        {
            Ledgers = new List<LedgerModel>();
            Ledger = new LedgerModel();
        }
        public List<LedgerModel> Ledgers { get; set; }
        public LedgerModel Ledger {  get; set; }
    }
}
