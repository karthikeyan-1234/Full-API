using API.Models;
using API.Models.DTOs;
using MediatR;

namespace API.DAL.Queries
{
    public class GetAllCitiesQuery: IRequest<IList<CityDTO>>
    {
    }
}
