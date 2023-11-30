namespace FMS.Db.DbEntity
{
    public class Group
    {
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
        public ICollection<SubGroup> SubGroups { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
