using Authenticate.Models;

namespace Authenticate.Services
{
    public interface ITenantService
    {
        Task<ResponseModel> AddNewTenantAsync(RegisterTenantModel tenant);
    }
}