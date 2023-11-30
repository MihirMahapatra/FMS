namespace FMS.Db.DbEntity
{
    public class Ledger
    {
        public Guid LedgerId { get; set; }
        public string LedgerName { get; set; }
        public string LedgerType { get;set; }
        public string HasSubLedger { get; set; }
        public Guid Fk_LedgerGroupId { get; set; }
        public Guid? Fk_LedgerSubGroupId { get; set; }
        public LedgerGroup LedgerGroup { get; set; }
        public LedgerSubGroup LedgerSubGroup { get; set; }
        public ICollection<SubLedger> SubLedgers { get; set; }
        public ICollection<LedgerBalance> LedgerBalances { get; set; }
        public ICollection<Party> Parties { get; set; }
        public ICollection<Journal> Journals { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<Receipt> Receipts { get; set; }

    }
}
