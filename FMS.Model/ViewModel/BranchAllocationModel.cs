using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class BranchAllocationModel : Base
    {
        public BranchAllocationModel() {
            Branches = new List<BranchModel>();
            Users=new List<UserModel>();
            UserBranch = new List<UserBranchModel>();
        }
        public List<BranchModel> Branches { get; set; }
        public List<UserModel> Users { get; set; }
        public List<UserBranchModel> UserBranch { get; set; }
    }
}
