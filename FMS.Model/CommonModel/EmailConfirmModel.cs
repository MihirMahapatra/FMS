namespace FMS.Model.CommonModel
{
    public class EmailConfirmModel : Base
    {
        public string Email { get; set; }
        public bool IsConfirmed { get; set; }
        public bool EmailSent { get; set; }
        public bool EmailVerified { get; set; }
    }
}
