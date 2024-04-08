namespace FMS.Db.DbEntity
{
    public class SalesReturnOrder
    {
        public Guid SalesReturnOrderId { get; set; }
        public string TransactionNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string PriceType { get; set; }
        public Guid? Fk_SubLedgerId { get; set; }
        public string PaymentMode { get; set; }
        public string CustomerName { get; set; } = null;
        public Guid Fk_BranchId { get; set; }
        public Guid Fk_FinancialYearId { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal Gst { get; set; }
        public string TranspoterName { get; set; }
        public string SiteAdress { get; set; }
        public decimal TransportationCharges { get; set; }
        public string VehicleNo { get; set; } = null;
        public string ReceivingPerson { get; set; } = null;
        public string Narration { get; set; } = null;
        public SubLedger SubLedger { get; set; }
        public Branch Branch { get; set; }
        public FinancialYear FinancialYear { get; set; }
        public ICollection<SalesReturnTransaction> SalesReturnTransactions { get; set; }
    }
}
