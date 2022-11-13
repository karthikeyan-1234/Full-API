using API.DAL.Queries;
using API.Infrastructure.Caching;
using API.Models;
using API.Models.DTOs;
using API.Models.ViewModels;
using API.Repositories;
using AutoMapper;
using MediatR;
using System.Reflection.Metadata.Ecma335;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.

namespace API.Services
{
    public class CityService : ICityService
    {
        IGenericRepo<City> repo;
        ILogger<CityService> logger;
        ICacheManager cache;
        IMapper mapper;
        string? user;
        ISessionService sessionService;
        IMediator mediator;

        public CityService(IGenericRepo<City> repo, IMapper mapper, ILogger<CityService> logger, ICacheManager cache,IMediator mediator,ISessionService sessionService)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.cache = cache;
            this.logger = logger;
            this.mediator = mediator;
            this.sessionService = sessionService;
            user = sessionService?.GetString("user");
        }

        public async Task<ResponseModel> AddCityAsync(CityDTO newCity)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                user = sessionService?.GetString("user");

                var newCi = mapper.Map<City>(newCity);

                var emp = await repo.AddAsync(newCi);
                await repo.SaveChangesAsync();
                logger.LogInformation("City {0} added by {1}", newCi.name, user);

                response.StatusCode = StatusCodes.Status201Created;
                response.Status = "Added";
                response.Message = "City added";
                response.Object = mapper.Map<CityDTO>(emp);
                return response;
            }
            catch (Exception ex)
            {
                logger.LogInformation("Unable to add City by user {0}", user);
                response.StatusCode = StatusCodes.Status500InternalServerError;
                response.Status = "Failed";
                response.Message = ex.Message;
                response.Object = newCity;
                return response;
            }
        }

        public async Task<ResponseModel> UpdateCityAsync(CityDTO City)
        {
            try
            {
                repo.Update(mapper.Map<City>(City));
                await repo.SaveChangesAsync();
                logger.LogInformation("City {0} deleted by {1}", City.name, user);
                return new ResponseModel()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Updated",
                    Status = "Updated",
                    Object = City
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    StatusCode = StatusCodes.Status304NotModified,
                    Message = ex.Message,
                    Status = "Update failed",
                    Object = City
                };
            }
        }

        public async Task<ResponseModel> DeleteCityAsync(CityDTO City)
        {
            try
            {
                repo.Delete(mapper.Map<City>(City));
                await repo.SaveChangesAsync();
                return new ResponseModel()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Deleted",
                    Status = "Deleted",
                    Object = City
                };
            }
            catch (Exception ex)
            {

                return new ResponseModel()
                {
                    StatusCode = StatusCodes.Status304NotModified,
                    Message = ex.Message,
                    Status = "Delete failed",
                    Object = City
                };
            }
        }

        public CityDTO GetCityByID(int id) => mapper.Map<CityDTO>(repo.Find(c => c.id == id).First());

        public CityDTO GetCityByName(string name) => mapper.Map<CityDTO>(repo.Find(c => c.name == name).First());

        public async Task<IList<CityDTO>> GetAllCitiesAsync()
        {
            IList<CityDTO> cities = await cache?.TryGetAsync<IList<CityDTO>>("GetAllCities");

            if (cities == null)
            {
                cities = await mediator.Send(new GetAllCitiesQuery());
                await cache.TrySetAsync("GetAllCities", cities);
            }

            logger.LogInformation("All cities requested by user {0}", user);
            return cities;
        }

        public ResponseModel GetCityById(int id)
        {
            var emp = repo.Find(e => e.id == id).FirstOrDefault();
            if (emp != null)
            {
                return new ResponseModel()
                {
                    Object = mapper.Map<CityDTO>(emp),
                    StatusCode = StatusCodes.Status302Found,
                    Message = "Found"
                };
            }
            else
                return new ResponseModel()
                {
                    Object = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Not Found"
                };
        }
    }
}


#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
