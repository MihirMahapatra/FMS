namespace FMS.Model.CommonModel
{
    public class ClaimsModel : UserClaimModel
    {
        public ClaimsModel()
        {
            Cliams = new List<UserClaimModel>();
        }
        public List<UserClaimModel> Cliams { get; set; }
    }
}
