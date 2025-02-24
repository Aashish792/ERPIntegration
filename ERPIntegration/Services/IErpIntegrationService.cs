using System.Threading.Tasks;

namespace ERPIntegration.Services
{
    public interface IErpIntegrationService
    {
        Task<string> GetTokenAsync();
        Task<string> GetCustomersJsonAsync(string token);
        Task LogoutAsync(string token); 
    }
}
