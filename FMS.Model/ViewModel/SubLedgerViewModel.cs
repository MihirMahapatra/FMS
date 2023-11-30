using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class SubLedgerViewModel : Base
    {
        public SubLedgerViewModel()
        {
            SubLedgers = new List<SubLedgerModel>();
            SubLedger = new SubLedgerModel();
        }
        public List<SubLedgerModel> SubLedgers { get; set; }
        public SubLedgerModel SubLedger { get; set; }
    }
}
