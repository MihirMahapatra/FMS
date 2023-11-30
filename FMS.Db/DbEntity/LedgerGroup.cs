namespace FMS.Db.DbEntity
{
    public class LedgerGroup
    {
        public Guid LedgerGroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupAlias { get; set; }
        public ICollection<LedgerSubGroup> LedgerSubGroups { get; set; }
        public ICollection<LedgerSubGroupDev> LedgerSubGroupsDev { get; set; }
        public ICollection<Ledger> Ledgers { get; set; }
        public ICollection<LedgerDev> LedgersDev { get; set; }
        public ICollection<Journal> Journals { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<Receipt> Receipts { get; set; }
    }
}

