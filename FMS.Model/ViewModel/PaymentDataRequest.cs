namespace FMS.Model.ViewModel
{
    public class PaymentDataRequest
    {
        public string CashBank { get; set; }
        public string BankLedgerId { get; set; } = null;
        public string ChqNo { get; set; } = null;
        public string ChqDate { get; set; } = null;
        public string VoucherNo { get; set; }
        public string VoucherDate { get; set; }
        public string Narration { get; set; } = null;  
        public List<PaymentRowData> arr { get; set; }
    }
    public class PaymentRowData
    {
        public string ddlLedgerId { get; set; }
        public string DrBalance { get; set; }
        public List<SubledgerPaymentData> subledgerData { get; set; }
    }
    public class SubledgerPaymentData
    {
        public List<string> ddlSubledgerId { get; set; }
        public List<string> SubledgerAmunt { get; set; }
    }
}
