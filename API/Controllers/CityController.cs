using API.Infrastructure.Filters;
using API.Models;
using API.Models.DTOs;
using API.Models.ViewModels;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

#pragma warning disable CS8604 // Possible null reference argument.


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "JWTBearer", Roles = "User")]
    [ServiceFilter(typeof(CustomSessionFilter))]
    public class CityController : ControllerBase
    {
        private IMapper mapper;
        private ICityService service;
        IStringLocalizer<CityController> localizer;

        public CityController(IMapper mapper,ICityService service, IStringLocalizer<CityController> localizer)
        {
            this.mapper = mapper;
            this.service = service;
            this.localizer = localizer;
        }

        [HttpGet("GetAllCities", Name = "GetAllCities")]
        public async Task<IActionResult> GetAllCities()
        {
            IList<CityDTO> cityDTOs = await service.GetAllCitiesAsync();

            for (int i = 0; i < cityDTOs.Count; i++)
            {
                cityDTOs[i].name = localizer[cityDTOs[i]?.name].Value;
            }

            return StatusCode(StatusCodes.Status200OK, cityDTOs);
        }

        [HttpGet("GetCityById", Name = "GetCityById")]
        public IActionResult GetCityById(int id)
        {
            var response = service.GetCityById(id);
            return StatusCode(response.StatusCode, response.Object);
        }


        [HttpPost("AddCity", Name = "AddCity")]
        public async Task<IActionResult> AddCity(CityDTO newCity)
        {
            var response = await service.AddCityAsync(newCity);
            return StatusCode(response.StatusCode, response.Object);
        }

        [HttpPut("UpdateCity", Name = "UpdateCity")]
        public async Task<IActionResult> UpdateCity(CityDTO newCity)
        {
            var response = await service.UpdateCityAsync(newCity);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("DeleteCity", Name = "DeleteCity")]
        public async Task<IActionResult> DeleteCity(CityDTO newCity)
        {
            var response = await service.DeleteCityAsync(newCity);
            return StatusCode(response.StatusCode, response.Status);
        }
    }
}
