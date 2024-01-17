using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class LabourTransactionViewModel
    {
        public LabourTransactionViewModel()
        {
            LabourTransactions = new List<LabourTransactionModel>();
            LabourTransaction = new LabourTransactionModel();
        }
        public List<LabourTransactionModel> LabourTransactions { get; set; }
        public LabourTransactionModel LabourTransaction { get; set; }
    }
}
