using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class ProductGroupViewModel : Base
    {
        public ProductGroupViewModel()
        {
            ProductGroups = new List<ProductGroupModel>();
            ProductGroup = new ProductGroupModel();
        }
        public List<ProductGroupModel> ProductGroups { get; set; }
        public ProductGroupModel ProductGroup { get; set; }
    }
}
