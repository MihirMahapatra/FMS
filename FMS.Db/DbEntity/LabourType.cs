namespace FMS.Db.DbEntity
{
    public class LabourType
    {
        public Guid LabourTypeId { get; set; }
        public string Labour_Type { get; set; }
        public ICollection<Labour> Labours { get; set; }
    }
}
