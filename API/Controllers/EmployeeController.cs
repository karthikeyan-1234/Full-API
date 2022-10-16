using API.Models;
using API.Models.DTOs;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Primitives;

#pragma warning disable CS8604 // Possible null reference argument.


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private IEmployeeService service;
        IStringLocalizer<EmployeeController> localizer;


        public EmployeeController(IEmployeeService service,IStringLocalizer<EmployeeController> localizer)
        {
            this.service = service;
            this.localizer = localizer;
        }

        [Authorize(AuthenticationSchemes = "JWTBearer",Roles = "User")]
        [HttpGet("GetAllEmployees",Name = "GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            IList<EmployeeDTO> employeeDTOs = await service.GetAllEmployeesAsync();

            for (int i = 0; i < employeeDTOs.Count; i++)
            {
                employeeDTOs[i].name = localizer[employeeDTOs[i]?.name].Value;
            }

            return StatusCode(StatusCodes.Status200OK,employeeDTOs);
        }

        [Authorize(AuthenticationSchemes = "JWTBearer", Roles = "User")]
        [HttpPost("AddEmployee",Name = "AddEmployee")]
        public async Task<IActionResult> AddEmployee(EmployeeDTO newEmp)
        {
            var emp = await service.AddEmployeeAsync(newEmp);
            return StatusCode(StatusCodes.Status200OK,emp);
        }
    }
}


#pragma warning restore CS8604 // Possible null reference argument.
