using FMS.Model.CommonModel;

namespace FMS.Model.ViewModel
{
    public class LabourTypeViewModel : Base
    {
        public LabourTypeViewModel()
        {
            LabourTypes = new List<LabourTypeModel>();
            LabourType = new LabourTypeModel();
        }
        public List<LabourTypeModel> LabourTypes { get; set; }
        public LabourTypeModel LabourType { get; set; }
    }
}
