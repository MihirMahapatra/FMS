namespace FMS.Db.DbEntity
{
    public class Labour
    {
        public Guid LabourId { get; set; }
        public string LabourName { get; set; }
        public Guid? Fk_Labour_TypeId { get; set; }
        public Guid Fk_SubLedgerId { get; set; }
        public Guid? Fk_BranchId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Reference { get; set; }
        public LabourType LabourType { get; set; }
        public Branch Branch { get; set; }
        public SubLedger SubLedger { get; set; }
        public ICollection<LabourOrder> LabourOrders { get; set; }
        public ICollection<DamageOrder> DamageOrders { get; set; }
    }
}
