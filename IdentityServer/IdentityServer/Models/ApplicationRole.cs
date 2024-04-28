using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace IdentityServer.Models
{
    public class ApplicationRole: IdentityRole<Guid>
    {
        public static ApplicationRole User => new()
        {
            Name = Constants.Role.User,
            NormalizedName = nameof(User).ToUpper(CultureInfo.InvariantCulture),
        };

        public static ApplicationRole Admin => new()
        {
            Name = Constants.Role.Admin,
            NormalizedName = nameof(Admin).ToUpper(CultureInfo.InvariantCulture)
        };
    }

    public static class Constants
    {
        public static class Role
        {
            public const string Admin = "admin";
            public const string User = "user";
        }

        public static string IdentityRoleName => "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
    }
}
