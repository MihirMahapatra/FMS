﻿namespace FMS.Db.DbEntity
{
    public class SalesReturnTransaction
    {
        public Guid SalesReturnId { get; }
        public Guid Fk_SalesReturnOrderId { get; set; }
        public string TransactionNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public Guid Fk_ProductId { get; set; }
        public Guid? Fk_SubProductId { get; set; }
        public Guid Fk_BranchId { get; set; }
        public Guid Fk_FinancialYearId { get; set; }
        public decimal AlternateQuantity { get; set; }
        public Guid? Fk_AlternateUnitId { get; set; }
        public decimal UnitQuantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Gst { get; set; }
        public decimal GstAmount { get; set; }
        public decimal Amount { get; set; }
        public SalesReturnOrder SalesReturnOrder { get; set; }
        public Product Product { get; set; }
        public Product SubProduct { get; set; }
        public Branch Branch { get; set; }
        public FinancialYear FinancialYear { get; set; }
        public AlternateUnit AlternateUnit { get; set; }
    }
}
