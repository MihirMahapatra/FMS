namespace FMS.Model.CommonModel
{
    public class StockReportSummerizedModel
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
        public decimal OpeningQty { get; set; }
        public string UnitName { get; set; }
    }
    public class StockReportDetailedModel
    {
        public DateTime TransactionDate { get; set; }
        public string TransactionNo { get; set; }
        public string Particular { get; set; }
        public decimal Quantity { get; set; }
        public string BranchName { get; set; }
        public bool IncrementStock { get; set; }
    }
    public class StockReportDetailedModel2
    {
        public StockReportDetailedModel2()
        {
            Stocks =new List<StockReportDetailedModel>();
        }
        public decimal OpeningQty { get; set; }
        public string BranchName { get; set; }
        public List<StockReportDetailedModel> Stocks { get; set; }
    }
    public class StockReportDetailedViewModel : Base
    {
        public StockReportDetailedViewModel()
        {
            DetailedStock = new List<StockReportDetailedModel2>();
        }
        public List<StockReportDetailedModel2> DetailedStock { get; set; }
    }
}
