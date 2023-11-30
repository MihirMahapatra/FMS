namespace FMS.Db.DbEntity
{
    public class RegisterToken
    {
        public Guid TokenId { get; set; }
        public string TokenValue { get; set; }
        public AppUser User { get; set; }
    }
}
