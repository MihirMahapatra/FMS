namespace FMS.Model.CommonModel
{
    public class SalesDataRequest
    {
        public string TransactionType { get; set; }
        public Guid SalesOrderId { get; set; }
        public string TransactionNo { get; set; }
        public Guid? Fk_SubLedgerId { get; set; }
        public string CustomerName { get; set; }
        public string TransactionDate { get; set; }
        public string OrderNo { get; set; }
        public string OrderDate { get; set; }
        public string TranspoterName { get; set; }
        public string VehicleNo { get; set; } = null;
        public string ReceivingPerson { get; set; } = null;
        public decimal SubTotal { get; set; }
        public decimal Gst { get; set; }
        public decimal Discount { get; set; }
        public decimal GrandTotal { get; set; }
        public List<List<string>> RowData { get; set; }
        public List<List<string>> PrintData { get; set; }
    }
}
