namespace FMS.Db.DbEntity
{
    public class PurchaseReturnOrder
    {
        public Guid PurchaseReturnOrderId { get; set; }
        public Guid Fk_ProductTypeId { get; set; }
        public string TransactionNo { get; set; }
        public Guid Fk_SubLedgerId { get; set; }
        public Guid Fk_BranchId { get; set; }
        public Guid Fk_FinancialYearId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TransportationCharges { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal Gst { get; set; }
        public string TranspoterName { get; set; }
        public string VehicleNo { get; set; } = null;
        public string ReceivingPerson { get; set; } = null;
        public string Narration { get; set; } = null;
        public SubLedger SubLedger { get; set; }
        public Branch Branch { get; set; }
        public FinancialYear FinancialYear { get; set; }
        public ProductType ProductType { get; set; }
        public ICollection<PurchaseReturnTransaction> PurchaseReturnTransactions { get; set; }
    }
}
