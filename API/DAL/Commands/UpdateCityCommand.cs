using API.Models;
using API.Models.DTOs;
using MediatR;

namespace API.DAL.Commands
{
    public class UpdateCityCommand : IRequest<City>
    {
        public City? city { get; set; }

        public UpdateCityCommand(City? city)
        {
            this.city = city;
        }
    }
}
