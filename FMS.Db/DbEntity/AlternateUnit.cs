namespace FMS.Db.DbEntity
{
    public class AlternateUnit
    {
        public Guid AlternateUnitId { get; set; }
        public string AlternateUnitName { get; set; }
        public decimal AlternateQuantity {  get; set; }
        public Guid FK_ProductId { get; set; }
        public Guid Fk_UnitId {  get; set; }
        public decimal UnitQuantity { get; set; }
        public decimal WholeSalePrice { get; set; }
        public decimal RetailPrice { get; set; }
        public Product Product { get; set; }
        public Unit Unit { get; set; }
        public  ICollection<PurchaseTransaction> PurchaseTransactions { get; set;}
        public ICollection<SalesTransaction> SalesTransactions {get; set;}
        public ICollection<SalesReturnTransaction> SalesReturnTransactions { get; set; }
        public ICollection<PurchaseReturnTransaction> PurchaseReturnTransactions { get; set; }
    }
}
