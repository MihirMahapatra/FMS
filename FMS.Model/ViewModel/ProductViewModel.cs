using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class ProductViewModel : Base
    {
        public ProductViewModel()
        {
            Products = new List<ProductModel>();
            Product = new ProductModel();
        }

        public List<ProductModel> Products { get; set; }
        public ProductModel Product { get; set; }
    }
}
