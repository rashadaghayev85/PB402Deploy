using Microsoft.AspNetCore.Mvc;
using Organic_Food_MVC_Project.ViewModels.Home;

namespace Organic_Food_MVC_Project.Services.Interfaces
{
    public interface IProductCategoryService
    {
        Task<IEnumerable<CategoryVM>> GetAllAsync();
    }
}
