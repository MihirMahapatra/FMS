namespace FMS.Model.CommonModel
{
    using System.Security.Claims;
    public static class ClaimsStoreModel
    {
        public static List<Claim> AllClaims = new()
        {
        new Claim("Create Role", "Create Role"),

        new Claim("Edit Role","Edit Role"),

        new Claim("Delete Role","Delete Role")
        };
    }
}
