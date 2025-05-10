using System.ComponentModel.DataAnnotations;

namespace Organic_Food_MVC_Project.Areas.Admin.ViewModels.Account
{
    public class DeleteRoleVM
    {
        [Required]
        public string Role { get; set; }
    }
}
