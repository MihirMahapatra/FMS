namespace FMS.Db.DbEntity
{
    public class ProductType
    {
        public Guid ProductTypeId { get; set; }
        public string Product_Type { get; set; }
        public ICollection<Group> Groups { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<InwardSupplyOrder> InwardSupplyOrders { get; set; }
        public ICollection<OutwardSupplyOrder> OutwardSupplyOrders { get; set; }
        public ICollection<DamageOrder> DamageOrders { get; set; }
    }
}
