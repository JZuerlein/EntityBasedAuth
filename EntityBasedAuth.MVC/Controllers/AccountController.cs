using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EntityBasedAuth.MVC.Controllers
{
    public class AccountController : Controller
    {
        const string ISSUER = "MyOrganization";

        public async Task<IActionResult> LoginAsJane(string returnUrl = null)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Sid, "JaneAccountName", ClaimValueTypes.String, ISSUER));
            claims.Add(new Claim(ClaimTypes.Name, "Jane", ClaimValueTypes.String, ISSUER));
            claims.Add(new Claim(ClaimTypes.Role, "", ClaimValueTypes.String, ISSUER));
            claims.Add(new Claim("UserId", "4", ClaimValueTypes.Integer, ISSUER));

            var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddMinutes(10),
                IsPersistent = false,
                AllowRefresh = false
            });

            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task<IActionResult> LoginAsSupervisor(string returnUrl = null)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Sid, "SupervisorAccountName", ClaimValueTypes.String, ISSUER));
            claims.Add(new Claim(ClaimTypes.Name, "Supervisor", ClaimValueTypes.String, ISSUER));
            claims.Add(new Claim(ClaimTypes.Role, "", ClaimValueTypes.String, ISSUER));
            claims.Add(new Claim("UserId", "1", ClaimValueTypes.Integer, ISSUER));

            var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddMinutes(10),
                IsPersistent = false,
                AllowRefresh = false
            });

            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task<IActionResult> LoginAsHR(string returnUrl = null)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Sid, "HRAccountName", ClaimValueTypes.String, ISSUER));
            claims.Add(new Claim(ClaimTypes.Name, "Human Resources", ClaimValueTypes.String, ISSUER));
            claims.Add(new Claim(ClaimTypes.Role, "HumanResources", ClaimValueTypes.String, ISSUER));
            claims.Add(new Claim("UserId", "5", ClaimValueTypes.Integer, ISSUER));

            var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddMinutes(10),
                IsPersistent = false,
                AllowRefresh = false
            });

            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
