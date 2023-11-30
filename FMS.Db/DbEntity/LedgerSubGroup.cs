namespace FMS.Db.DbEntity
{
    public class LedgerSubGroup
    {
        public Guid LedgerSubGroupId { get; set; }
        public Guid Fk_LedgerGroupId { get; set; }
        public Guid Fk_BranchId { get; set; }
        public string SubGroupName { get; set; }
        public LedgerGroup LedgerGroup { get; set; }
        public Branch Branch { get; set; }
        public ICollection<Ledger> Ledgers { get; set; }
    }
}
