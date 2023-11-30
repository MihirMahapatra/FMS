using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class LedgerSubGroupViewModel : Base
    {
        public LedgerSubGroupViewModel()
        {
            LedgerSubGroups = new List<LedgerSubGroupModel>();
            LedgerSubGroup = new LedgerSubGroupModel();
        }
        public List<LedgerSubGroupModel> LedgerSubGroups { get; set; }
        public LedgerSubGroupModel LedgerSubGroup { get; set; }
    }
}
