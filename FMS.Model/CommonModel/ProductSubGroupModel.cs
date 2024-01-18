namespace FMS.Model.CommonModel
{
    public class ProductSubGroupModel : Base
    {
        public Guid ProductSubGroupId { get; set; }
        public Guid Fk_ProductGroupId { get; set; }
        public string ProductSubGroupName { get; set; }
        public ProductGroupModel ProductGroup { get; set; }
        public List<ProductModel> Products { get; set; } 
    }
}
