namespace FMS.Model.CommonModel
{
    public class LedgerSubGroupModel
    {
        public Guid LedgerSubGroupId { get; set; }
        public Guid Fk_LedgerGroupId { get; set; }
        public Guid Fk_BranchId { get; set; }
        public string SubGroupName { get; set; }
        public LedgerGroupModel LedgerGroup { get; set; } 
        public BranchModel Branch { get; set; } 
        public List<LedgerModel> Ledgers { get; set; }
    }
}
