using API.DAL.Commands;
using API.Models;
using API.Models.DTOs;
using API.Repositories;
using MediatR;

namespace API.DAL.Handlers
{
    public class UpdateCityCommandHandler : IRequestHandler<UpdateCityCommand, City>
    {
        IGenericRepo<City> repo;

        public UpdateCityCommandHandler(IGenericRepo<City> repo)
        {
            this.repo = repo;
        }

        public async Task<City> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
        {
            repo.Update(request?.city);
            await repo.SaveChangesAsync();
            return request.city;
        }
    }
}
