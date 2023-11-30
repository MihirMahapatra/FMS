using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class SubGroupViewModel : Base
    {
        public SubGroupViewModel()
        {
            SubGroups = new List<SubGroupModel>();
            SubGroup = new SubGroupModel();
        }
        public List<SubGroupModel> SubGroups { get; set; }
        public SubGroupModel SubGroup { get; set; }
    }
}
