using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class StockViewModel : Base
    {
        public StockViewModel() {
            Stocks = new List<StockModel>();
            Stock = new StockModel();
        }
        public List<StockModel> Stocks { get; set; }
        public StockModel Stock { get; set; }
    }
}
