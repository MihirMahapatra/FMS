using System.ComponentModel.DataAnnotations.Schema;

namespace FMS.Db.DbEntity
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal? WholeSalePrice { get; set; }
        public decimal GST { get; set; }
        public Guid Fk_ProductTypeId { get; set; }
        public Guid Fk_UnitId { get; set; }
        public Guid Fk_ProductGroupId { get; set; }
        public Guid? Fk_ProductSubGroupId { get; set; }
        public ProductType ProductType { get; set; }
        public ProductGroup ProductGroup { get; set; }
        public ProductSubGroup ProductSubGroup { get; set; }
        public Unit Unit { get; set; }
        public ICollection<AlternateUnit> AlternateUnits { get; set; }
        public ICollection<LabourRate> LabourRates { get; set; }
        public ICollection<Stock> Stocks { get; set; }
        public ICollection<LabourOrder> LabourOrders { get; set; }
        public ICollection<LabourTransaction> LabourTransactions { get; set; }
        public ICollection<PurchaseTransaction> PurchaseTransactions { get; set; }
        public ICollection<PurchaseReturnTransaction> PurchaseReturnTransactions { get; set; }
        public ICollection<SalesTransaction> SalesTransactions { get; set; }
        public ICollection<SalesReturnTransaction> SalesReturnTransactions { get; set; }
        public ICollection<InwardSupplyTransaction> InwardSupplyTransactions { get; set; }
        public ICollection<OutwardSupplyTransaction> OutwardSupplyTransactions { get; set; }
        public ICollection<DamageTransaction> DamageTransactions { get; set; }
    }
}
