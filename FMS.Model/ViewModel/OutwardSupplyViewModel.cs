using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class OutwardSupplyViewModel:Base
    {
        public OutwardSupplyViewModel()
        {
            OutwardSupplies = new List<OutwardSupplyOrderModel>();
            OutwardSupply = new OutwardSupplyOrderModel();
        }
        public List<OutwardSupplyOrderModel> OutwardSupplies { get; set; }
        public OutwardSupplyOrderModel OutwardSupply { get; set; }
    }
}
