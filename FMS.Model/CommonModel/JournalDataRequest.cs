namespace FMS.Model.CommonModel
{
    public class SubledgerData
    {
        public List<string> ddlSubledgerId { get; set; }
        public List<string> SubledgerAmunt { get; set; }
    }

    public class JournalRowData
    {
        public string BalanceType { get; set; }
        public string ddlLedgerId { get; set; }
        public string DrBalance { get; set; }
        public string CrBalance { get; set; }
        public List<SubledgerData> subledgerData { get; set; }
    }

    public class JournalDataRequest
    {
        public string VoucherNo { get; set; }
        public string VoucherDate { get; set; }
        public string Narration { get; set; }
        public List<JournalRowData> arr { get; set; }
    }
}
