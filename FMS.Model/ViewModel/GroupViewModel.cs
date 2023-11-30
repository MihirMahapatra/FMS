using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class GroupViewModel : Base
    {
        public GroupViewModel()
        {
            Groups = new List<GroupModel>();
            Group = new GroupModel();
        }
        public List<GroupModel> Groups { get; set; }
        public GroupModel Group { get; set; }
    }
}
