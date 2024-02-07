namespace FMS.Model.CommonModel
{
    public class PurchaseDataRequest
    {
        public Guid PurchaseOrderId { get; set; }
        public Guid Fk_ProductTypeId { get; set; }
        public Guid Fk_SubLedgerId { get; set; }
        public string TransactionNo { get; set; }
        public string TransactionDate { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string TranspoterName { get; set; }
        public string VehicleNo { get; set; }
        public string ReceivingPerson { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public string Naration { get; set; }
        public decimal TransportationCharges { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal GstAmount { get; set; }
        public List<List<string>> RowData { get; set; }
    }
}
