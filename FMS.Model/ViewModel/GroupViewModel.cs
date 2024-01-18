using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class GroupViewModel : Base
    {
        public GroupViewModel()
        {
            Groups = new List<ProductGroupModel>();
            Group = new ProductGroupModel();
        }
        public List<ProductGroupModel> Groups { get; set; }
        public ProductGroupModel Group { get; set; }
    }
}
