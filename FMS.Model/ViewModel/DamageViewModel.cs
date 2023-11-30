using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class DamageViewModel : Base
    {
        public DamageViewModel()
        {
            DamageOrders = new List<DamageOrderModel>();
            DamageOrder = new DamageOrderModel();
        }
        public List<DamageOrderModel> DamageOrders { get; set; }
        public DamageOrderModel DamageOrder { get; set; }
    }
}
