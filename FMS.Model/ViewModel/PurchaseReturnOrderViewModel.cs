using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class PurchaseReturnOrderViewModel : Base
    {
        public PurchaseReturnOrderViewModel()
        {
            purchaseReturnOrder = new PurchaseReturnOrderModel();
            purchaseReturnOrders = new List<PurchaseReturnOrderModel>();
        }
        public PurchaseReturnOrderModel purchaseReturnOrder { get; set; }
        public List<PurchaseReturnOrderModel> purchaseReturnOrders { get; set; }
    }
}
