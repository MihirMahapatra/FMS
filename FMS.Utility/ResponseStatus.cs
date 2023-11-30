using System.ComponentModel;
using System.Reflection;

namespace FMS.Utility
{
    public class ResponseStatus
    {
        public enum Status
        {
            [Description("error")]
            Error = 0,
            [Description("success")]
            Success = 1,
            [Description("created")]
            Created = 2,
            [Description("modified")]
            Modified = 3,
            [Description("deleted")]
            Deleted = 4,
            [Description("found")]
            Found = 5,
            [Description("notfound")]
            NotFound = 6,
        }
    }
    public static class ResponseStatusExtensions
    {
        public static string ToStatusString(this ResponseStatus.Status status)
        {
            var memberInfo = typeof(ResponseStatus.Status).GetMember(status.ToString());
            var descriptionAttribute = memberInfo[0].GetCustomAttribute<DescriptionAttribute>();
            return descriptionAttribute != null ? descriptionAttribute.Description : status.ToString();
        }
    }
}
