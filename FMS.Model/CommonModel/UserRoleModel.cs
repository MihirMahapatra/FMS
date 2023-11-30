using System.ComponentModel.DataAnnotations;

namespace FMS.Model.CommonModel
{
    public class UserRoleModel
    {
        public UserRoleModel()
        {
            Cliams = new List<UserClaimModel>();
        }
        public string Id { get; set; }

        [Required]
        public string RoleName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool IsRoleSelected { get; set; }
        public List<UserClaimModel> Cliams { get; set; }
    }
}
