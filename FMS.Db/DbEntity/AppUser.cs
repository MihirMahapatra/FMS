using Microsoft.AspNetCore.Identity;

namespace FMS.Db.DbEntity
{
    public class AppUser : IdentityUser
    {
        public Guid FkTokenId { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Photo { get; set; }
        public bool? IsActive { get; set; }
        public RegisterToken Token { get; set; }
        public IList<UserBranch> UserBranch { get; set; }
    }
}
