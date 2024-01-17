using System.ComponentModel.DataAnnotations.Schema;

namespace FMS.Db.DbEntity
{
    public class Production
    {
        public Guid ProductionId { get; set; }
        public Guid Fk_FinishedGoodId { get; set; }
        public Guid Fk_RawMaterialId { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
    }
}
