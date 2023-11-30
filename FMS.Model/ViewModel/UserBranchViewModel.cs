using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class UserBranchViewModel : Base
    {
        public UserBranchViewModel() {
            UserBranchs = new List<UserBranchModel>();
            UserBranch = new UserBranchModel();
        }
        public List<UserBranchModel> UserBranchs { get; set; }
        public UserBranchModel UserBranch { get; set; }
    }
}
