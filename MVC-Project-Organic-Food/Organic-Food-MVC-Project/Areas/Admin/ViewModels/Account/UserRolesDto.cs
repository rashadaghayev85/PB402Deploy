namespace Organic_Food_MVC_Project.Areas.Admin.ViewModels.Account
{
    public class UserRolesDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public List<String> Roles { get; set; }
    }
}
