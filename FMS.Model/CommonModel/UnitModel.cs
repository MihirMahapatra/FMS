namespace FMS.Model.CommonModel
{
    public class UnitModel : Base
    {
        public Guid UnitId { get; set; }
        public string UnitName { get; set; }
        public List<ProductModel> Products { get; set; } 
    }
}
