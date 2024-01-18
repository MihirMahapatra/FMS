using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class ProductSubGroupViewModel : Base
    {
        public ProductSubGroupViewModel()
        {
            ProductSubGroups = new List<ProductSubGroupModel>();
            ProductSubGroup = new ProductSubGroupModel();
        }
        public List<ProductSubGroupModel> ProductSubGroups { get; set; }
        public ProductSubGroupModel ProductSubGroup { get; set; }
    }
}
