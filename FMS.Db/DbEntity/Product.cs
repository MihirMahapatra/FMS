using System.ComponentModel.DataAnnotations.Schema;

namespace FMS.Db.DbEntity
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal GST { get; set; }
        public Guid Fk_ProductTypeId { get; set; }
        public Guid Fk_UnitId { get; set; }
        public Guid Fk_GroupId { get; set; }
        public Guid? Fk_SubGroupId { get; set; }
        public ProductType ProductType { get; set; }
        public Group Group { get; set; }
        public SubGroup SubGroup { get; set; }
        public Unit Unit { get; set; }
        public ICollection<LabourRate> LabourRates { get; set; }
        public ICollection<Stock> Stocks { get; set; }
        [InverseProperty("FinishedGood")]
        public ICollection<Production> FinishedGoodProductions { get; set; }
        [InverseProperty("RawMaterial")]
        public ICollection<Production> RawMaterialProductions { get; set; }
        public ICollection<ProductionEntry> ProductionEntries { get; set; }
        public ICollection<ProductionEntryTransaction> ProductionEntryTransactions { get; set; }
        public ICollection<PurchaseTransaction> PurchaseTransactions { get; set; }
        public ICollection<PurchaseReturnTransaction> PurchaseReturnTransactions { get; set; }
        public ICollection<SalesTransaction> SalesTransactions { get; set; }
        public ICollection<SalesReturnTransaction> SalesReturnTransactions { get; set; }
        public ICollection<InwardSupplyTransaction> InwardSupplyTransactions { get; set; }
        public ICollection<OutwardSupplyTransaction> OutwardSupplyTransactions { get; set; }
        public ICollection<DamageTransaction> DamageTransactions { get; set; }
    }
}
