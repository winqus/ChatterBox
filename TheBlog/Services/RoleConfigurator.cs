using Microsoft.AspNetCore.Identity;
using System.Data;
using TheBlog.Common.Constants;

namespace TheBlog.Services
{
    public class RoleConfigurator
    {
        private readonly List<string> _roles = typeof(UserRoles).GetFields().Select(f => f.Name).ToList();

        private readonly RoleManager<IdentityRole> roleManager;

        public RoleConfigurator(RoleManager<IdentityRole> manager)
        {
            roleManager = manager;
        }
        public async Task CreateDefaultRoles()
        {
            foreach (var role in _roles)
            {
                if (roleManager.FindByNameAsync(role).Result == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
