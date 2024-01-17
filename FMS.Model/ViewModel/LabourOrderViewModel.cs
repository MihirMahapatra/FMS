using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class LabourOrderViewModel : Base
    {
        public LabourOrderViewModel()
        {
            LabourOrders = new List<LabourOrderModel>();
            LabourOrder = new LabourOrderModel();
        }
        public List<LabourOrderModel> LabourOrders { get; set; }
        public LabourOrderModel LabourOrder { get; set; }
    }
}
