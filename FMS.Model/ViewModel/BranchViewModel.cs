using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class BranchViewModel : Base
    {
        public BranchViewModel()
        {
            Branches = new List<BranchModel>();
            Branch = new BranchModel();
        }
        public List<BranchModel> Branches { get; set; }
        public BranchModel Branch { get; set; }
    }
}
