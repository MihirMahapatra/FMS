using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class ProductionViewModel : Base
    {
        public ProductionViewModel()
        {
            Productions = new List<ProductionModel>();
            Production = new ProductionModel();
        }
        public List<ProductionModel> Productions { get; set; }
        public ProductionModel Production { get; set; }
    }
}
