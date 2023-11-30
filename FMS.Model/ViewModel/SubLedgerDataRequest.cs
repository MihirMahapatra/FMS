namespace FMS.Model.ViewModel
{
    public class SubLedgerDataRequest
    {
       public Guid Fk_LedgerId { get; set; }
        public string SubLedgerName { get; set; }
        public decimal OpeningBalance { get; set; }
        public string BalanceType { get; set; }
    }
}
