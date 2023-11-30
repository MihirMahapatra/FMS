namespace FMS.Model.CommonModel
{
    public class DamageRequestData
    {
        public Guid? DamageId { get; set; }
        public string TransactionNo { get; set; }
        public string TransactionDate { get; set; }
        public Guid Fk_ProductTypeId { get; set; }
        public Guid? Fk_LabourId { get; set; }
        public string Reason { get; set; }
        public decimal TotalAmount { get; set; }
        public List<List<string>> RowData { get; set; }
    }
}
