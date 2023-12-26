namespace FMS.Model.CommonModel
{
    public class CashBookModal
    {
        public string VouvherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public string FromDate { get; set; }
        public string Todate { get; set; }
        public string Narration { get; set; }
        public string PartyName { get; set; }
        public decimal OpeningBal { get; set; }
        public decimal ClosingBal { get; set; }
        public string OpeningBalType { get; set; }
        public decimal DrCrAmt { get; set; }
        public string DrCr { get; set; }
        public decimal CrAmt { get; set; }
        public decimal Balance { get; set; }
        public string ToAcc { get; set; }
        public decimal Amount { get; set; }
        public string BalanceType { get; set; }
        public List<ReceiptModel> Receipts { get; set; }
        public List<PaymentModel> Payments { get; set; }
        public List<JournalModel> Journals { get; set; }
        public Dictionary<DateTime, List<TransactionModel>> AllTransactionsByDate { get; set; }

        public CashBookModal()
        {
            AllTransactionsByDate = new Dictionary<DateTime, List<TransactionModel>>();
        }
    }
}
