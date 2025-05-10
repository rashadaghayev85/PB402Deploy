using System.ComponentModel.DataAnnotations;

namespace Organic_Food_MVC_Project.ViewModels.Account
{
    public class RegisterVM
    {
        [Required]
        public string FullName { get; set; }
        [Required]

        public string Username { get; set; }
        [Required]
        [EmailAddress,DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password),Compare(nameof(Password))]
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
