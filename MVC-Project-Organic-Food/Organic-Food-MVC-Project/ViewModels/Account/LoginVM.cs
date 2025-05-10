using System.ComponentModel.DataAnnotations;

namespace Organic_Food_MVC_Project.ViewModels.Account
{
    public class LoginVM
    {
        [Required]
        public string UsernameOrEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
