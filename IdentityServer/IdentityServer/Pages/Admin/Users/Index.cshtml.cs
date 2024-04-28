using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IdentityServer.Models;

namespace IdentityServer.Pages.Admin.Users
{
    public class UsersModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public IEnumerable<UsersModel> Users { get; private set; } = default!;
        public void OnGet()
        {
            Users = _userManager.Users.Select(e=> new UsersModel
            {
                Name = e.UserName,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber
            }).ToList();
        }
    }
}
