using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TheBlog.Models;

namespace TheBlog.Filters
{
    public class UserInRoleAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string _role;

        private readonly string _errorMessage;

        public UserInRoleAttribute(string role, string errorMessage)
        {
            _role = role;
            _errorMessage = errorMessage;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userId = context.HttpContext.User.Identity.GetSubjectId();

            var userManager = (UserManager<ApplicationUser>?)context.HttpContext.RequestServices.GetService(typeof(UserManager<ApplicationUser>));

            var user = userManager?.FindByIdAsync(userId).Result;

            if (userManager != null && user != null)
            {
                var isInRole = userManager.IsInRoleAsync(user, _role).Result;

                if (!isInRole)
                {
                    context.ModelState.AddModelError("Prohibited", _errorMessage);
                    context.Result = new BadRequestObjectResult(context.ModelState);
                }
            }
        }
    }
}
