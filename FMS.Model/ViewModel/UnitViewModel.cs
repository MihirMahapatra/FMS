using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class UnitViewModel : Base
    {
        public UnitViewModel() {
            Units = new List<UnitModel>();
            Unit= new UnitModel();
        }
        public List<UnitModel> Units { get; set; }
        public UnitModel Unit { get; set; }
    }
}
