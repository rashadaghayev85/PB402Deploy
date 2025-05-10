using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace OrganicFood_MiniProject.Areas.Admin.ViewModels.Account
{
    public class AddRoleToUserDto
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        [Required]
        public List<SelectListItem> Roles { get; set; }
    }
}
