namespace FMS.Model.CommonModel
{
    public class StockReportDataRequest
    {
        public Guid ProductTypeId { get; set; }
        public Guid ProductId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ZeroValued { get; set; }
    }
}
