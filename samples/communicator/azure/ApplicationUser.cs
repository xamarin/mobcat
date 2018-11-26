using Microsoft.AspNetCore.Identity;

namespace ChatRoom
{
    public class ApplicationUser : IdentityUser
    {
        public virtual string Email { get; set; } // example, not necessary
    }
}