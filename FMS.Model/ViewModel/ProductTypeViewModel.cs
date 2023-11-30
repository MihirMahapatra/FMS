using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class ProductTypeViewModel : Base
    {
        public ProductTypeViewModel()
        {
            ProductTypes = new List<ProductTypeModel>();
            ProductType = new ProductTypeModel();
        }
        public List<ProductTypeModel> ProductTypes { get; set; }
        public ProductTypeModel ProductType { get; set; }
    }
}
