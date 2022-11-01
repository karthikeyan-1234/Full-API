using API.Models;
using API.Models.DTOs;
using API.Repositories;
using AutoMapper;
using System.Reflection.Metadata.Ecma335;

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

        public CityDTO GetCityByID(int id) => mapper.Map<CityDTO>(repo.Find(c => c.id == id).First());

        public CityDTO GetCityByName(string name) => mapper.Map<CityDTO>(repo.Find(c => c.name == name).First());
    }
}
