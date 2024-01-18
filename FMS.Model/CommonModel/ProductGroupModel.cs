namespace FMS.Model.CommonModel
{
    public class ProductGroupModel : Base
    {
        public Guid ProductGroupId { get; set; }
        public string ProductGroupName { get; set; }
        public Guid Fk_ProductTypeId { get; set; }
        public ProductTypeModel ProductType { get; set; }
        public List<ProductSubGroupModel> ProductSubGroups { get; set; } 
        public List<ProductModel> Products { get; set; }
    }
}
