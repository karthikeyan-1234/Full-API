using API.Models;
using API.Models.DTOs;
using API.Models.ViewModels;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;

#pragma warning disable CS8604 // Possible null reference argument.


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "JWTBearer", Roles = "User")]
    public class EmployeeController : ControllerBase
    {
        private IEmployeeService service;
        IStringLocalizer<EmployeeController> localizer;

        public EmployeeController(IEmployeeService service,IStringLocalizer<EmployeeController> localizer)
        {
            this.service = service;
            this.localizer = localizer;
        }

        [HttpGet("GetAllEmployees",Name = "GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            ResponseModel response = await service.GetAllEmployeesAsync();
            IList<EmployeeViewModel>? employeeDTOs = null;

            if (response.Object != null)
            {
                employeeDTOs = (IList<EmployeeViewModel>)response.Object;

                for (int i = 0; i < employeeDTOs.Count; i++)
                {
                    employeeDTOs[i].name = localizer[employeeDTOs[i]?.name].Value;
                }
            }

            return StatusCode(response.StatusCode,employeeDTOs);
        }
        [HttpGet("GetEmployeeById", Name = "GetEmployeeById")]
        public IActionResult GetEmployeeById(int id)
        {
            var response = service.GetEmployeeById(id);
            return StatusCode(response.StatusCode, response.Object);
        }

        [HttpPost("AddEmployee",Name = "AddEmployee")]
        public async Task<IActionResult> AddEmployee(EmployeeViewModel newEmp)
        {
            var response = await service.AddEmployeeAsync(newEmp);
            return StatusCode(response.StatusCode,response.Object);
        }

        [HttpPut("UpdateEmployee", Name = "UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee(EmployeeViewModel newEmp)
        {
            var response = await service.UpdateEmployeeAsync(newEmp);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("DeleteEmployee", Name = "DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee(EmployeeViewModel newEmp)
        {
            var response = await service.DeleteEmployeeAsync(newEmp);
            return StatusCode(response.StatusCode, response.Status);
        }
    }
}


#pragma warning restore CS8604 // Possible null reference argument.
