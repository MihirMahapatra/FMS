namespace FMS.Model.CommonModel
{
    public class RegisterTokenModel : Base
    {
        public Guid TokenId { get; set; }
        public string TokenValue { get; set; }
    }
}
