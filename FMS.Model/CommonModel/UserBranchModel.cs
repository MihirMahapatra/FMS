namespace FMS.Model.CommonModel
{
    public class UserBranchModel : Base
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid BranchId { get; set; }
        public UserModel User { get; set; } 
        public BranchModel Branch { get; set; } 
    }
}
