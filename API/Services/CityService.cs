using API.Models;
using API.Models.DTOs;
using API.Repositories;
using AutoMapper;

namespace API.Services
{
    public class CityService : ICityService
    {
        IGenericRepo<City> repo;
        IMapper mapper;

        public CityService(IGenericRepo<City> repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<CityDTO> AddCityAsync(CityDTO newCity)
        {
            var city = await repo.AddAsync(mapper.Map<City>(newCity));
            await repo.SaveChangesAsync();
            return mapper.Map<CityDTO>(city.Entity);
        }

        public CityDTO GetCityByID(int id)
        {
            return mapper.Map<CityDTO>(repo.Find(c => c.id == id).First());
        }
    }
}
