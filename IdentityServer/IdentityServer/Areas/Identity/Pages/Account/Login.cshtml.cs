// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IdentityServer.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace IdentityServer.Areas.Identity.Pages.Account
{
    public class LoginOptions
    {
        public static bool AllowLocalLogin = true;
        public static bool AllowRememberLogin = true;
        public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);
        public static string InvalidCredentialsErrorMessage = "Invalid username or password";
    }

    public class InputModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ViewModel
    {
        public bool AllowRememberLogin { get; set; } = true;
        public bool EnableLocalLogin { get; set; } = true;
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
    }

    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public ViewModel View { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public string ReturnUrl { get; set; }

        public LoginModel(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            View = new ViewModel();
        }

        public async Task<IActionResult> OnGet(string returnUrl = "/")
        {
            ReturnUrl = returnUrl;

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            View.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            View.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(Input.Username);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(Input.Username, Input.Password, Input.RememberMe, false);
                    if (result.Succeeded)
                    {
                        var claims = new List<Claim>
                        {
                            new(ClaimTypes.Email, Input.Username),
                        };

                        var principal = new ClaimsPrincipal(new List<ClaimsIdentity>
                        {
                            new(claims, CookieAuthenticationDefaults.AuthenticationScheme)
                        });

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                        return Redirect(ReturnUrl);
                    }
                    if (result.RequiresTwoFactor)
                    {
                        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                    }
                    if (result.IsLockedOut)
                    {
                        return RedirectToPage("./Lockout");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return Page();
                    }
                }
            }

            return Page();
        }
    }
}
