using API.Models;
using API.Models.DTOs;
using API.Models.ViewModels;

namespace API.Services
{
    public interface IEmployeeService
    {
        Task<ResponseModel> AddEmployeeAsync(EmployeeViewModel newEmp);
        Task<IList<EmployeeViewModel>> GetAllEmployeesAsync();
        Task<IList<EmployeeDTO>> GetAllEmployeesWithoutCacheAsync();
        Task<ResponseModel> UpdateEmployeeAsync(EmployeeViewModel nEmp);
        Task<ResponseModel> DeleteEmployeeAsync(EmployeeViewModel emp);
        ResponseModel GetEmployeeById(int id);
    }
}