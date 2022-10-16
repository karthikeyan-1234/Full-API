using API.Models;
using API.Models.DTOs;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private IMapper mapper;
        private ICityService service;

        public CityController(IMapper mapper,ICityService service)
        {
            this.mapper = mapper;
            this.service = service;
        }

        [HttpPost("AddCity",Name = "AddCity")]
        public async Task<IActionResult> AddCity(CityDTO newEmp)
        {
            var emp = await service.AddCityAsync(newEmp);
            return StatusCode(StatusCodes.Status200OK,emp);
        }
    }
}
