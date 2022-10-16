using Authenticate.Models;

namespace Authenticate.Services
{
    public interface ILoginService
    {
        Task<ResponseModel> RegisterAdminAsync(RegisterModel model);
        Task<ResponseModel?> RegisterUserAsync(RegisterModel model);
        Task<ResponseModel?> RegisterOperatorAsync(RegisterModel model);        
    }
}