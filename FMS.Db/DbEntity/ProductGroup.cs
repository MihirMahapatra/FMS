namespace FMS.Db.DbEntity
{
    public class ProductGroup
    {
        public Guid ProductGroupId { get; set; }
        public string ProductGroupName { get; set; }
        public Guid Fk_ProductTypeId { get; set; }
        public ProductType ProductType { get; set; }
        public ICollection<ProductSubGroup> ProductSubGroups { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
