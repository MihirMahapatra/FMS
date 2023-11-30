using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class ProductionEntryTransactionViewModel
    {
        public ProductionEntryTransactionViewModel()
        {
            ProductionEntryTransactions = new List<ProductionEntryTransactionModel>();
            ProductionEntryTransaction = new ProductionEntryTransactionModel();
        }
        public List<ProductionEntryTransactionModel> ProductionEntryTransactions { get; set; }
        public ProductionEntryTransactionModel ProductionEntryTransaction { get; set; }
    }
}
