using Microsoft.AspNetCore.Identity;

namespace ProductivityTools.Meetings.WebApi.Controllers
{
    public class ContextUser
    {
        public string  Email { get; set; }
    }

    public class ApplicationUser : IdentityUser
    {
        public string CustomTag { get; set; }
    }
}