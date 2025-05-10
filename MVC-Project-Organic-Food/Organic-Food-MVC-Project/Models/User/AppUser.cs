using Microsoft.AspNetCore.Identity;

namespace Organic_Food_MVC_Project.Models.User
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
