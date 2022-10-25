using API.Models.DTOs;

namespace API.Services
{
    public interface ICityService
    {
        Task<CityDTO> AddCityAsync(CityDTO newCity);
        CityDTO GetCityByID(int id);
    }
}