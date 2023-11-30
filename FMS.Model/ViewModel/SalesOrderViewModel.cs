using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class SalesOrderViewModel : Base
    {
        public SalesOrderViewModel()
        {
            salesOrders = new List<SalesOrderModel>();
            salesOrder = new SalesOrderModel();
        }
        public List<SalesOrderModel> salesOrders { get; set; }
        public SalesOrderModel salesOrder { get; set; }
    }
}
