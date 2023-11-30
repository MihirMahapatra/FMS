using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class ProductViewModel : Base
    {
        public ProductViewModel()
        {
            products = new List<ProductModel>();
            product = new ProductModel();
        }

        public List<ProductModel> products { get; set; }
        public ProductModel product { get; set; }
    }
}
