namespace FMS.Db.DbEntity
{
    public class City
    {
        public Guid CityId { get; set; }
        public Guid Fk_StateId { get; set; }
        public Guid Fk_BranchId { get; set; }
        public string CityName { get; set; }
        public State State { get; set; }
        public Branch Branch { get; set; }
        public ICollection<Party> Parties { get; set; }
    }
}
