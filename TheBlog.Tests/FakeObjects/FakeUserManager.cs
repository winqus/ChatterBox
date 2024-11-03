using Microsoft.AspNetCore.Identity;
using Moq;
using TheBlog.Models;

namespace TheBlog.Tests.MockObjects
{
    public class FakeUserManager : UserManager<ApplicationUser>
    {
        public FakeUserManager() : base(new Mock<IUserStore<ApplicationUser>>().Object,
            null, null, null, null, null, null, null, null)
        {

        }
    }
}
