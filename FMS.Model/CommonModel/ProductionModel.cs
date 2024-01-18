namespace FMS.Model.CommonModel
{
    public class ProductionModel : Base
    {
        public Guid ProductionId { get; set; }
        public Guid Fk_FinishedGoodId { get; set; }
        public Guid Fk_RawMaterialId { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }

        //Other
        public string ProductName { get; set; }
        public string RawMaterialName { get; set; }
        public string FinishedGoodName { get; set; }
        public ProductModel FinishedGood { get; set; } 
        public ProductModel RawMaterial { get; set; } 
    }
}
