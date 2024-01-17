namespace FMS.Model.CommonModel
{
    public class LabourModel : Base
    {
        public Guid LabourId { get; set; }
        public string LabourName { get; set; }
        public Guid Fk_Labour_TypeId { get; set; }
        public Guid Fk_SubLedgerId { get; set; }
        public Guid Fk_BranchId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Reference { get; set; }
        public decimal OpeningBalance { get; set; }
        public string BalanceType { get; set; }
        public LabourTypeModel LabourType { get; set; }
        public BranchModel Branch { get; set; }
        public SubLedgerModel SubLedger { get; set; }
        public List<LabourOrderModel> LabourOrders { get; set; }
        public List<DamageOrderModel> DamageOrders { get; set; }
        public List<PaymentModel> Payment {  get; set; }
    }
}
