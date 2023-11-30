namespace FMS.Model.CommonModel
{
    public class ChangePasswordModel : Base
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
