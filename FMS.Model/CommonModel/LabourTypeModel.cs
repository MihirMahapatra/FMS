namespace FMS.Model.CommonModel
{
    public class LabourTypeModel : Base
    {
        public Guid LabourTypeId { get; set; }
        public string Labour_Type { get; set; }
        public List<LabourModel> Labours { get; set; } 
    }
}
