using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Client.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;
using System.Security.Claims;
using OpenIddict.Abstractions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using IdentityServer.Models;
using Polly;

namespace IdentityServer.Controllers
{
    [ApiController]
    public class ExternalCallbackController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public ExternalCallbackController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost("~/signin-google"), HttpGet("~/signin-google"), IgnoreAntiforgeryToken]
        public async Task<ActionResult> LogInCallback(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            return await ProcessSuccessfulExternalLogin(returnUrl);
        }

        private async Task<ActionResult> ProcessSuccessfulExternalLogin(string returnUrl)
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(OpenIddictClientAspNetCoreDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
            {
                return RedirectToLoginPage(returnUrl);
            }

            if (!authenticateResult.Principal?.Identity?.IsAuthenticated ?? false)
            {
                throw new InvalidOperationException("The external authorization data cannot be used for authentication.");
            }

            var email = authenticateResult.Principal.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                throw new InvalidOperationException("Email claim not found.");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = CreateUser();
                user.Email = email;
                user.UserName = email;
                var userCreateResult = await _userManager.CreateAsync(user);
                if (!userCreateResult.Succeeded)
                {
                    throw new InvalidOperationException($"Failed to create user: {string.Join(", ", userCreateResult.Errors.Select(error => error.Description))}");
                }
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.SetClaim(ClaimTypes.Email, email)
                    .SetClaim(ClaimTypes.Name, authenticateResult.Principal.GetClaim(ClaimTypes.Name))
                    .SetClaim(ClaimTypes.NameIdentifier, authenticateResult.Principal.GetClaim(ClaimTypes.NameIdentifier))
                    .SetClaim(Claims.Private.RegistrationId, authenticateResult.Principal.GetClaim(Claims.Private.RegistrationId));

            var properties = new AuthenticationProperties(authenticateResult.Properties.Items)
            {
                RedirectUri = authenticateResult.Properties.RedirectUri ?? "/"
            };

            var tokensToStore = authenticateResult.Properties.GetTokens().Where(token =>
                token.Name is OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessToken or
                OpenIddictClientAspNetCoreConstants.Tokens.BackchannelIdentityToken or
                OpenIddictClientAspNetCoreConstants.Tokens.RefreshToken);

            properties.StoreTokens(tokensToStore);

            await _signInManager.SignInAsync(user, false, CookieAuthenticationDefaults.AuthenticationScheme);

            return SignIn(new ClaimsPrincipal(identity), properties, CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private ActionResult RedirectToLoginPage(string returnUrl)
        {
            return RedirectToPage("/Identity/Account/Login", new { ReturnUrl = returnUrl });
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the external login page in /Areas/Identity/Pages/Account/ExternalLogin.cshtml");
            }
        }
    }
}
