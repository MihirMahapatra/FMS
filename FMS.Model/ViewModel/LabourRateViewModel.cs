using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class LabourRateViewModel : Base
    {
        public LabourRateViewModel()
        {
            LabourRates = new List<LabourRateModel>();
            LabourRate = new LabourRateModel();
        }
        public List<LabourRateModel> LabourRates { get; set; }
        public LabourRateModel LabourRate { get; set; }
    }
}
