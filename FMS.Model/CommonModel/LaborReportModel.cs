namespace FMS.Model.CommonModel
{
    public class LaborReportModel
    {
        public string LabourName { get; set; }
        public decimal OpeningBal { get;set; }
        public string OpeningBalType { get;set; }
        public decimal BillingAmt { get;set; }
        public decimal PaymentAmt { get;set; }
        public decimal DamageAmt { get; set; }
        public List<LabourOrderModel> ProductionEntries { get; set; }
        public List<DamageOrderModel> DamageOrders { get; set; }
        public List<PaymentModel> Payment { get; set; }
        
    }

    public class LobourReportDetailedModel
    {
        public DateTime TransactionDate { get; set; }
        public string TransactionNo { get; set; }
        public string Particular { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal OTAmount { get; set; }
        public decimal Amount { get; set; }
        public string BranchName { get; set; }
        public bool IncrementStock { get; set; }
        public ProductModel Product { get; set; }
        public string Narration { get; set; }
    }

    public class LabourReportDetailedModel2
    {
        public LabourReportDetailedModel2()
        {
            Orders = new List<LobourReportDetailedModel>();
        }
        public decimal OpeningBalance { get; set; }
        public string BalanceType { get; set; }
        public string BranchName { get; set; }
        public string LabourName { get; set; }
        public List<LobourReportDetailedModel> Orders { get; set; }
    }
    public class LobourReportDetailedViewModel : Base
    {
        public LobourReportDetailedViewModel()
        {
            DetailedLabour = new List<LabourReportDetailedModel2>();
        }
        public List<LabourReportDetailedModel2> DetailedLabour { get; set; }
    }







}
