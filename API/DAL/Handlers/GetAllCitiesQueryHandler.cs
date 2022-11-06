using API.DAL.Queries;
using API.Models;
using API.Models.DTOs;
using API.Repositories;
using AutoMapper;
using MediatR;

namespace API.DAL.Handlers
{
    public class GetAllCitiesQueryHandler : IRequestHandler<GetAllCitiesQuery, IList<CityDTO>>
    {
        IGenericRepo<City> repo;
        IMapper mapper;

        public GetAllCitiesQueryHandler(IGenericRepo<City> repo,IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }


        public async Task<IList<CityDTO>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
        {
            var result = mapper.Map<IList<CityDTO>>(await repo.GetAllAsync());
            return result;
        }
    }
}
