using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class InwardSupplyViewModel:Base
    {
        public InwardSupplyViewModel() 
        {
            InwardSupplies = new List<InwardSupplyOrderModel>();
            InwardSupply =new InwardSupplyOrderModel();
        }
        public List<InwardSupplyOrderModel> InwardSupplies { get; set; }
        public InwardSupplyOrderModel InwardSupply { get; set; }
    }
}
