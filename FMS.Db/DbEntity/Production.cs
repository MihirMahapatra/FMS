using System.ComponentModel.DataAnnotations.Schema;

namespace FMS.Db.DbEntity
{
    public class Production
    {
        public Guid ProductionId { get; set; }
        public Guid FinishedGoodId { get; set; }
        public Guid RawMaterialId { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        [NotMapped]
        [ForeignKey("FinishedGoodId")]
        public Product FinishedGood { get; set; }
        [NotMapped]
        [ForeignKey("RawMaterialId")]
        public Product RawMaterial { get; set; }
    }
}
