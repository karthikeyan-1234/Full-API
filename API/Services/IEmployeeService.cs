using API.Models.DTOs;

namespace API.Services
{
    public interface IEmployeeService
    {
        Task<EmployeeDTO> AddEmployeeAsync(EmployeeDTO newEmp);
        Task<IList<EmployeeDTO>> GetAllEmployeesAsync();
    }
}