using Organic_Food_MVC_Project.ViewModels.Home;

namespace Organic_Food_MVC_Project.Services.Interfaces
{
    public interface IAdvertisementService
    {
        Task<AdvertisementVM> GetAsync();
    }
}
