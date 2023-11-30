using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class PurchaseOrderViewModel : Base
    {
        public PurchaseOrderViewModel()
        {
            purchaseOrders = new List<PurchaseOrderModel>();
            purchaseOrder = new PurchaseOrderModel();
        }
        public List<PurchaseOrderModel> purchaseOrders { get; set; }
        public PurchaseOrderModel purchaseOrder { get; set; }
    }
}
