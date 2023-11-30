using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class StateViewModel : Base
    {
        public StateViewModel()
        {
            States = new List<StateModel>();
            State = new StateModel();
        }
        public List<StateModel> States { get; set; }
        public StateModel State { get; set; }
    }
}
