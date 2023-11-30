namespace FMS.Model.CommonModel
{
    public class RoleModel : UserRoleModel
    {
        public RoleModel()
        {
            Users = new List<UserRoleModel>();
        }
        public List<UserRoleModel> Users { get; set; }
    }
}
