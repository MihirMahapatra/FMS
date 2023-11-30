namespace FMS.Db.DbEntity
{
    //many-to-many Relation
    public class UserBranch
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public Guid BranchId { get; set; }
        public Branch Branch { get; set; }
    }
}
