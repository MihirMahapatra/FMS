namespace FMS.Model.CommonModel
{
    using FMS.Model;
    public class ForgotPasswordModel : Base
    {
        public string Email { get; set; }
        public bool EmailSent { get; set; }
    }
}
