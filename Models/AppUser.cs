using Microsoft.AspNetCore.Identity;

namespace Fitness.Models
{
    public class AppUser : IdentityUser
    {
        public string Fullname { get; set; } = string.Empty;
    }
}
