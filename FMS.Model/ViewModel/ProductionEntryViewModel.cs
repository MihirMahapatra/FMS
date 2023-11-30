using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class ProductionEntryViewModel : Base
    {
        public ProductionEntryViewModel()
        {
            ProductionEntries = new List<ProductionEntryModel>();
            ProductionEntry = new ProductionEntryModel();
        }
        public List<ProductionEntryModel> ProductionEntries { get; set; }
        public ProductionEntryModel ProductionEntry { get; set; }
    }
}
