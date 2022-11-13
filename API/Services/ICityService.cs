using API.Models;
using API.Models.DTOs;

namespace API.Services
{
    public interface ICityService
    {
        Task<ResponseModel> AddCityAsync(CityDTO newCity);
        CityDTO GetCityByID(int id);

        CityDTO GetCityByName(string name);
        Task<ResponseModel> UpdateCityAsync(CityDTO City);
        Task<ResponseModel> DeleteCityAsync(CityDTO City);
        Task<IList<CityDTO>> GetAllCitiesAsync();

        ResponseModel GetCityById(int id);
    }
}