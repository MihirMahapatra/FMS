namespace FMS.Model.CommonModel
{
    public class GroupModel : Base
    {
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
        public Guid? Fk_ProductTypeId { get; set; }
        public ProductTypeModel ProductType { get; set; }
        public List<SubGroupModel> subGroups { get; set; } 
        public List<ProductModel> Products { get; set; }
    }
}
