namespace FMS.Model.CommonModel
{
    public class ProductTypeModel : Base
    {
        public Guid ProductTypeId { get; set; }
        public string Product_Type { get; set; }
        public List<GroupModel> Groups { get; set; }
        public List<ProductModel> Products { get; set; }
        public List<InwardSupplyOrderModel> InwardSupplyOrders { get; set; }
        public List<OutwardSupplyOrderModel> OutwardSupplyOrders { get; set; }
        public List<DamageOrderModel> DamageOrders { get; set; }

    }
}
