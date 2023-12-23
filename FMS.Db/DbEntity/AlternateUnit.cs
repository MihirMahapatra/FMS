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
        public Product Product { get; set; }
        public Unit Unit { get; set; }
    }
}
