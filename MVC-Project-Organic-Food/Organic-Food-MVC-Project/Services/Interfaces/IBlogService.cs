using Organic_Food_MVC_Project.ViewModels.Blog;

namespace Organic_Food_MVC_Project.Services.Interfaces
{
    public interface IBlogService
    {
        Task<IEnumerable<BlogVM>> GetAllAsync();
    }
}
