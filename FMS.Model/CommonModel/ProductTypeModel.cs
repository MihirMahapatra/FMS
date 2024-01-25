using FMS.Db.DbEntity;

namespace FMS.Model.CommonModel
{
    public class ProductTypeModel : Base
    {
        public Guid ProductTypeId { get; set; }
        public string Product_Type { get; set; }
        public List<ProductGroupModel> ProductGroups { get; set; }
        public List<ProductModel> Products { get; set; }
        public List<PurchaseOrderModel> PurchaseOrders { get; set; }
        public List<PurchaseReturnOrderModel> PurchaseReturnOrders { get; set; }
        public List<LabourRateModel> LabourRates { get; set; }
        public List<InwardSupplyOrderModel> InwardSupplyOrders { get; set; }
        public List<OutwardSupplyOrderModel> OutwardSupplyOrders { get; set; }
        public List<DamageOrderModel> DamageOrders { get; set; }

    }
}
