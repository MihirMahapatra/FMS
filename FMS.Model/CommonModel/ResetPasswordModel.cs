namespace FMS.Model.CommonModel
{

    using System.ComponentModel.DataAnnotations;
    public class ResetPasswordModel : Base
    {

        public string UserId { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }
        public bool IsSuccess { get; set; }
    }
}
