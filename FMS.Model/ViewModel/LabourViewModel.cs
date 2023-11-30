using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class LabourViewModel : Base
    {
        public LabourViewModel()
        {
            Labours = new List<LabourModel>();
            Labour = new LabourModel();
        }
        public List<LabourModel> Labours { get; set; }
        public LabourModel Labour { get; set; }
    }
}
