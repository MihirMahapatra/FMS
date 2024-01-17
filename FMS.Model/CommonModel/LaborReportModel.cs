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
}
