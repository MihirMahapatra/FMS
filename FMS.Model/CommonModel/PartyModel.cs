namespace FMS.Model.CommonModel
{
    public class PartyModel : Base
    {
        public Guid PartyId { get; set; }
        public Guid Fk_PartyType { get; set; }
        public Guid Fk_SubledgerId { get; set; }
        public Guid Fk_StateId { get; set; }
        public Guid Fk_CityId { get; set; }
        public Guid Fk_BranchId { get; set; }
        public string PartyName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string GstNo { get; set; }
        public int CreditLimit { get; set; }
        public decimal OpeningBalance { get; set; }
        public string BalanceType { get; set; }
        public PartyTypeModel PartyType { get; set; }
        public StateModel State { get; set; }
        public CityModel City { get; set; }
        public BranchModel Branch { get; set; }
        public LedgerModel Ledger { get; set; }
        public SubLedgerModel SubLedger { get; set; }
        public SubLedgerBalanceModel SubLedgerBalance { get; set; }
        public decimal OpeningBal { get; set; }
        public string OpeningBalType { get; set; }
        public List<PurchaseOrderModel> PurchaseOrders { get; set; }
        public List<PurchaseReturnOrderModel> PurchaseReturns { get; set; }
        public List<SalesOrderModel> SalesOrders { get; set; }
        public List<SalesReturnOrderModel> SalesReturns { get; set; }
        public List<SalesTransactionModel> SalesTransactions { get; set; }
        public List<ReceiptModel> Receipts { get; set; }
        public List<PaymentModel> payments { get; set; }
    }
}
