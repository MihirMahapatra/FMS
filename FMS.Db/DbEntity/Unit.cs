namespace FMS.Db.DbEntity
{
    public class Unit
    {
        public Guid UnitId { get; set; }
        public string UnitName { get; set; }
        public ICollection<Product> Products { get; set; }

    }
}
