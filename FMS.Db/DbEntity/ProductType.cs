namespace FMS.Db.DbEntity
{
    public class ProductType
    {
        public Guid ProductTypeId { get; set; }
        public string Product_Type { get; set; }
        public ICollection<ProductGroup> ProductGroups { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<PurchaseOrder> PurchaseOrders { get; set; }
        public ICollection<PurchaseReturnOrder> PurchaseReturnOrders { get; set; }
        public ICollection<LabourRate> LabourRates { get; set; }
        public ICollection<InwardSupplyOrder> InwardSupplyOrders { get; set; }
        public ICollection<OutwardSupplyOrder> OutwardSupplyOrders { get; set; }
        public ICollection<DamageOrder> DamageOrders { get; set; }
    }
}
