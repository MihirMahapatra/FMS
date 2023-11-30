namespace FMS.Model.CommonModel
{
    public class SupplyDataRequest
    {
        public Guid SupplyId { get; set; }
        public string TransactionNo { get; set; }
        public string TransactionDate { get; set; }
        public Guid Branch { get; set; }
        public Guid Fk_ProductTypeId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<List<string>> RowData { get; set; }
    }
}
