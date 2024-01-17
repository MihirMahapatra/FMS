namespace FMS.Db.DbEntity
{
    public class Party
    {
        public Guid PartyId { get; set; }
        public Guid Fk_PartyType { get; set; }
        public Guid? Fk_SubledgerId { get; set; }
        public Guid Fk_StateId { get; set; }
        public Guid Fk_CityId { get; set; }
        public string PartyName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string GstNo { get; set; }
        public int CreditLimit { get; set; }
        public State State { get; set; }
        public City City { get; set; }
        public LedgerDev LedgerDev { get; set; }
        public SubLedger SubLedger { get; set; }
    }
}
