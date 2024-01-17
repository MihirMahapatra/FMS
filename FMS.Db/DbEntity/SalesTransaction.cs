namespace FMS.Db.DbEntity
{
    public class SalesTransaction
    {
        public Guid SalesId { get; }
        public Guid Fk_SalesOrderId { get; set; }
        public string TransactionNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public Guid Fk_ProductId { get; set; }
        public Guid Fk_BranchId { get; set; }
        public Guid Fk_FinancialYearId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Gst { get; set; }
        public decimal GstAmount { get; set; }
        public decimal Amount { get; set; }
        public Branch Branch { get; set; }
        public FinancialYear FinancialYear { get; set; }
        public SalesOrder SalesOrder { get; set; }
        public Product Product { get; set; }

    }
}
