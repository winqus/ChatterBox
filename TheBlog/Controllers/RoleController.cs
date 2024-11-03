using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheBlog.Common.Constants;
using TheBlog.Filters;
using TheBlog.Models;
using TheBlog.Models.AccountViewModels;
using TheBlog.ViewModels.RoleViewModels;

namespace TheBlog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoles.Admin)]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        
        [HttpGet("GetAllUserRoles")]
        public async Task<IActionResult> GetAllUserRoles()
        {
            var users = await _userManager.Users.ToListAsync();
            var model = new List<UserRolesViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                model.Add(new UserRolesViewModel
                {
                    UserName = user.UserName!,
                    UserId = user.Id,
                    Roles = roles.ToList(),
                });
            }

            return Ok(model);
        }

        [HttpGet("GetAllRoles")]
        public IActionResult GetAllRoles()
        {
            var list = _roleManager.Roles.Select(r => r.Name).OrderBy(r => r).ToList();
            return Ok(list);
        }


        [ValidateModel]
        [HttpPut("Update")]
        [UserInRole(UserRoles.Admin, "User does not have permission to change roles.")]
        public async Task<IActionResult> Update([FromForm] UpdateUserRoleViewModel model)
        {
            var user = _userManager.FindByIdAsync(model.UserId).Result;
            if (user == null)
            {
                ModelState.AddModelError("UserId", "No user with such id.");
                return BadRequest(ModelState);
            }

            var role = _roleManager.FindByNameAsync(model.RoleName).Result;
            if (role == null)
            {
                ModelState.AddModelError("RoleName", "No such role exists.");
                return BadRequest(ModelState);
            }

            if (role.Name == UserRoles.Admin
                && model.RoleStatus == false
                && _userManager.GetUsersInRoleAsync(UserRoles.Admin).Result.Count == 1)
            {
                ModelState.AddModelError("Error", "Cannot remove the last admin.");
                return BadRequest(ModelState);
            }

            if (model.RoleStatus == true)
            {
                await EnsureAddUserToRole(user, role.Name!);
            }
            else
            {
                await RemoveRoleFromUser(user, role.Name!);
            }

            return Ok(model);
        }

        [NonAction]
        public async Task EnsureAddUserToRole(ApplicationUser user, string role)
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

        [NonAction]
        public async Task RemoveRoleFromUser(ApplicationUser user, string role)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User can't be null.");
            }

            await _userManager.RemoveFromRoleAsync(user, role);
        }
    }
}
