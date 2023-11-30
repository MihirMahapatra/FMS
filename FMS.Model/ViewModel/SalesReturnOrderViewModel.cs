using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class SalesReturnOrderViewModel : Base
    {
        public SalesReturnOrderViewModel()
        {
            SalesReturnOrder = new SalesReturnOrderModel();
            SalesReturnOrders = new List<SalesReturnOrderModel>();
        }
        public SalesReturnOrderModel SalesReturnOrder { get; set; }
        public List<SalesReturnOrderModel> SalesReturnOrders { get; set; }
    }
}
