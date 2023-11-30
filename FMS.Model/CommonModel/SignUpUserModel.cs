using Microsoft.AspNetCore.Http;

namespace FMS.Model.CommonModel
{
    public class SignUpUserModel : Base
    {

        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public Guid FkTokenId { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string MaratialStatus { get; set; }
        public string GenderId { get; set; }
        public IFormFile ProfilePhoto { get; set; }
        public string PhotoPath { get; set; }
    }
}
