namespace FMS.Model.CommonModel
{
    public class ProductionEntryRequest
    {
        public string ProductionEntryId { get; set; }
        public string ProductionNo { get; set; }
        public string ProductionDate { get; set; }
        public List<List<string>> RowData { get; set; }
    }
}
