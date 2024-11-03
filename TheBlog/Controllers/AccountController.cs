using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheBlog.Models.AccountViewModels;
using TheBlog.Models;
using TheBlog.Filters;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TheBlog.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager { get; }

        private SignInManager<ApplicationUser> _signInManager { get; }

        private RoleManager<IdentityRole> _roleManager { get; }

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [ValidateModel]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (IsUserAuthenticated())
            {
                return Redirect("/authenticate/login");
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("Invalid login details.");
            }
            
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: true, lockoutOnFailure: false);

            if(result.Succeeded)
            {
                return Ok("Login successful!");
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Something failed.");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Logged out");
        }

        [ValidateModel]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (IsUserAuthenticated())
            {
                return Redirect("/authenticate/login");
            }

            if ((await _userManager.FindByEmailAsync(model.Email)) != null)
            {
                return BadRequest("Invalid registration attempt.");
            }

            var user = new ApplicationUser { Email = model.Email, UserName = model.Username };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (!(_roleManager.RoleExistsAsync("Admin").Result))
                {
                    await EnsureAddUserToRole(user, "Admin");
                }

                await EnsureAddUserToRole(user, "User");

                await _signInManager.SignInAsync(user, isPersistent: true);
                return Created(string.Empty, "Registered");
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Something failed.");
        }

        [ValidateModel]
        [HttpPost]
        public async Task<IActionResult> RestorePassword(RestorePasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action(
                    "ResetPassword",
                    "Account",
                    new { userId = user.Id, code = code },
                    protocol: HttpContext.Request.Scheme
                );

                // In place of mailing.
                return Json(new
                {
                    message = "Check your email for recovery link!",
                    username = user.UserName,
                    code,
                    callbackUrl
                });;
            }

            return Ok("Check your email for recovery link!");

        }

        [HttpGet]
        public IActionResult ResetPassword(string? userId, string? code)
        {
            if (userId == null || code == null)
            {
                return Redirect("/");
            }
            var url = $"/account/resetpassword?userId={HttpUtility.UrlEncode(userId)}&code={HttpUtility.UrlEncode(code)}";
            return Redirect(url);
        }

        [ValidateModel]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(PasswordResetViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                return BadRequest("Invalid details.");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return Ok("Password reset!");
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Something failed.");
        }

        [Authorize]
        [ValidateModel]
        [HttpPost]
        public async Task<IActionResult> Update(UpdateProfileViewModel model)
        {
            var user = await _userManager.FindByIdAsync(_userManager.GetUserId(User)!);
            
            if (user != null)
            {
                if ((await _userManager.CheckPasswordAsync(user!, model.Password)) == false)
                {
                    return Json(new { Password = new[] { "Invalid password." } });
                }

                if (model.NewEmail != null)
                {
                    await _userManager.SetEmailAsync(user, model.NewEmail);
                }

                if (model.NewUsername != null)
                {
                    await _userManager.SetUserNameAsync(user, model.NewUsername);
                }

                if (model.NewPassword != null)
                {
                    await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
                }

                return Ok("Account details updated.");
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Something failed.");
        }

        private bool IsUserAuthenticated() => (User.Identity != null) && (User.Identity.IsAuthenticated);

        private async Task EnsureAddUserToRole(ApplicationUser user, string role)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User can't be null.");
            }

            if (_roleManager.FindByNameAsync(role).Result == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            await _userManager.AddToRoleAsync(user, role);
        }
    }
}
