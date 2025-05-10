using Microsoft.AspNetCore.Mvc.Rendering;

namespace OrganicFood_MiniProject.Areas.Admin.ViewModels.Account
{
    public class DeleteRoleDto
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public List<SelectListItem> Roles { get; set; }
    }
}
