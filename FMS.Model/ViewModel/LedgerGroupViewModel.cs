using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class LedgerGroupViewModel : Base
    {
        public LedgerGroupViewModel()
        {
            LedgerGroups = new List<LedgerGroupModel>();
            LedgerGroup = new LedgerGroupModel();
        }
        public List<LedgerGroupModel> LedgerGroups { get; set; }
        public LedgerGroupModel LedgerGroup { get; set; }
    }
}
