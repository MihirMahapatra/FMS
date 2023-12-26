namespace FMS.Model.CommonModel
{
    public class StockReportModel
    {
        public string ProductName { get; set; }
        public decimal DamageQty { get; set; }
        public decimal OutwardQty { get; set; }
        public decimal InwardQty { get; set; }
        public decimal SalesQty { get; set; }
        public decimal SalesReturnQty { get; set; }
        public decimal PurchaseQty { get; set; }
        public decimal PurchaseReturnQty { get; set; }
        public decimal ProductionEntryQty { get; set; }
        public decimal ProductionQty { get; set; }
        public decimal StockQty { get; set; }
        public  decimal OpeningQty { get; set; }
        

    }
}
