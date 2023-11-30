namespace FMS.Model.CommonModel
{
    public class UserModel : Base
    {
        public string id { get; set; }
        public Guid FkTokenId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string MaratialStatus { get; set; }
        public string Gender { get; set; }
        public string Photo { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
