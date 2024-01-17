namespace FMS.Db.DbEntity
{
    public class ProductSubGroup
    {
        public Guid ProductSubGroupId { get; set; }
        public Guid? Fk_ProductGroupId { get; set; }
        public string ProductSubGroupName { get; set; }
        public ProductGroup ProductGroup { get; set; }
        public ICollection<Product> products { get; set; }   
    }
}
