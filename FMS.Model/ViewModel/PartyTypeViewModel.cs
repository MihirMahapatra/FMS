using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class PartyTypeViewModel : Base
    {
        public PartyTypeViewModel()
        {
            PartyTypes = new List<PartyTypeModel>();
            PartyType = new PartyTypeModel();
        }
        public List<PartyTypeModel> PartyTypes { get; set; }
        public PartyTypeModel PartyType { get; set; }
    }
}
