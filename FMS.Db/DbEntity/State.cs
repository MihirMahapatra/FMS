namespace FMS.Db.DbEntity
{
    public class State
    {
        public Guid StateId { get; set; }
        public string StateName { get; set; }
        public ICollection<Party> Parties { get; set; }
        public ICollection<City> Cities { get; set; }
    }
}
