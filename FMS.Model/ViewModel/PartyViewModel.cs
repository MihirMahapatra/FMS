using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class PartyViewModel : Base
    {
        public PartyViewModel()
        {
            Parties = new List<PartyModel>();
            Party = new PartyModel();
        }
        public List<PartyModel> Parties { get; set; }
        public PartyModel Party { get; set; }
    }
}
