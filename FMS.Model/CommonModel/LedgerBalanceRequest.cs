namespace FMS.Model.CommonModel
{
    public class LedgerBalanceRequest
    {
        public Guid Fk_LedgerId { get; set; }
        public decimal OpeningBalance { get; set; }
        public string OpeningBalanceType { get; set; }
    }
}
