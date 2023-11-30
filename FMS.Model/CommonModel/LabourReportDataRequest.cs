namespace FMS.Model.CommonModel
{
    public class LabourReportDataRequest
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public Guid LabourId { get; set; }
    }
}
