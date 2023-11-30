namespace FMS.Model.ViewModel
{
    public class LedgerDataRequest
    {
        public string LedgerGroupId { get; set; }
        public string LedgerSubGroupId { get; set; }
        public string BranchId { get; set; }
        public List<List<string>> RowData { get; set; }
    }
}
