namespace FMS.Db.DbEntity
{
    public class Group
    {
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
        public Guid? Fk_ProductTypeId { get; set; }
        public ProductType ProductType { get; set; }
        public ICollection<SubGroup> SubGroups { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
