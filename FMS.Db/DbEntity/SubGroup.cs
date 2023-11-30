namespace FMS.Db.DbEntity
{
    public class SubGroup
    {
        public Guid SubGroupId { get; set; }
        public Guid? Fk_GroupId { get; set; }
        public string SubGroupName { get; set; }
        public Group Group { get; set; }
        public ICollection<Product> products { get; set; }   
    }
}
