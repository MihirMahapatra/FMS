namespace FMS.Model.CommonModel
{
    public class ReciptsDataRequest
    {
        public string CashBank { get; set; }
        public string BankLedgerId { get; set; } = null;
        public string ChqNo { get; set; } = null;
        public string ChqDate { get; set; } = null;
        public string VoucherNo { get; set; }
        public string VoucherDate { get; set; }
        public string Narration { get; set; } = null;
        public List<ReciptsRowData> arr { get; set; }
    }
    public class ReciptsRowData
    {
        public string ddlLedgerId { get; set; }
        public string CrBalance { get; set; }
        public List<SubledgerReciptsData> subledgerData { get; set; }
    }
    public class SubledgerReciptsData
    {
        public List<string> ddlSubledgerId { get; set; }
        public List<string> SubledgerAmunt { get; set; }
    }
}
