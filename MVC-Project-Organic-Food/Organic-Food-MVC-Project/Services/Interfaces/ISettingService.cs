namespace Organic_Food_MVC_Project.Services.Interfaces
{
    public interface ISettingService
    {
        Task<Dictionary<string, string>> GetAllAsync();
    }
}
