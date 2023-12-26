namespace FMS.Db.DbEntity
{
    public class LabourRate
    {
        public Guid LabourRateId { get; set; }
        public DateTime? Date { get; set; }
        public Guid? Fk_ProductTypeId { get; set; }
        public Guid Fk_ProductId { get; set; }
        public decimal Rate { get; set; }
        public ProductType ProductType { get; set; }
        public Product Product { get; set; }
    }
}
