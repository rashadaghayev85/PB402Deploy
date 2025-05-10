using Microsoft.AspNetCore.Identity;

namespace OrganicFood_MiniProject.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
