namespace FMS.Model.CommonModel
{
    public class ProductModel
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal OpeningStock {  get; set; }
        public decimal Price { get; set; }
        public decimal? WholeSalePrice { get; set; }
        public decimal GST { get; set; }
        public Guid Fk_ProductTypeId { get; set; }
        public Guid Fk_UnitId { get; set; }
        public Guid Fk_GroupId { get; set; }
        public Guid? Fk_SubGroupId { get; set; }
        public ProductTypeModel ProductType { get; set; }
        public GroupModel Group { get; set; }
        public SubGroupModel SubGroup { get; set; }
        public UnitModel Unit { get; set; }
        public List<LabourRateModel> LabourRates { get; set; }
        public List<StockModel> Stocks { get; set; }
        public List<ProductionModel> FinishedGoodProductions { get; set; }
        public List<ProductionModel> RawMaterialProductions { get; set; }
        public List<ProductionEntryModel> ProductionEntries { get; set; }
        public List<ProductionEntryTransactionModel> ProductionEntryTransactions { get; set; }
        public List<PurchaseTransactionModel> PurchaseTransactions { get; set; }
        public List<SalesTransactionModel> SalesTransactions { get; set; }
        public List<SalesReturnTransactionModel> SalesReturnTransactions { get; set; }
        public List<PurchaseReturnTransactionModel> PurchaseReturnTransactions { get; set; }
        public List<InwardSupplyTransactionModel> InwardSupplyTransactions { get; set; }
        public List<OutwardSupplyTransactionModel> OutwardSupplyTransactions { get; set; }
        public List<DamageTransactionModel> DamageTransactions { get; set; }
        public decimal OpeningQty { get; set; }
    }
}
