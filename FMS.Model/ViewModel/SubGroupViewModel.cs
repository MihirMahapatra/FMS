using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class SubGroupViewModel : Base
    {
        public SubGroupViewModel()
        {
            SubGroups = new List<ProductSubGroupModel>();
            SubGroup = new ProductSubGroupModel();
        }
        public List<ProductSubGroupModel> SubGroups { get; set; }
        public ProductSubGroupModel SubGroup { get; set; }
    }
}
