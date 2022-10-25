using API.Models.DTOs;
using API.Models.ViewModels;

namespace API.Services
{
    public interface IEmployeeService
    {
        Task<EmployeeDTO> AddEmployeeAsync(EmployeeDTO newEmp);
        Task<IList<EmployeeViewModel>> GetAllEmployeesAsync();
        Task<IList<EmployeeDTO>> GetAllEmployeesWithoutCacheAsync();
    }
}