using API.Infrastructure.Caching;
using API.Models;
using API.Models.DTOs;
using API.Models.ViewModels;
using API.Repositories;
using AutoMapper;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.

namespace API.Services
{
    public class EmployeeService : IEmployeeService
    {
        IGenericRepo<Employee> repo;
        ICityService cityService;
        IMapper mapper;
        ICacheManager cache;
        ILogger<EmployeeService> logger;
        IHttpContextAccessor accessor;
        string? user;

        public EmployeeService(IGenericRepo<Employee> repo, IMapper mapper, ICacheManager cache, ILogger<EmployeeService> logger, IHttpContextAccessor accessor, ICityService cityService)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.cache = cache;
            this.logger = logger;
            this.accessor = accessor;
            var session = accessor?.HttpContext?.Session;
            user = session?.GetString("user");
            this.cityService = cityService;
        }

        public async Task<EmployeeDTO> AddEmployeeAsync(EmployeeDTO newEmp)
        {

            var session = accessor?.HttpContext?.Session;
            user = session?.GetString("user");

            var emp = await repo.AddAsync(mapper.Map<Employee>(newEmp));
            await repo.SaveChangesAsync();
            logger.LogInformation("Employee {} added by {}", newEmp.name, user);
            return mapper.Map<EmployeeDTO>(emp.Entity);
        }

        public async Task<IList<EmployeeViewModel>> GetAllEmployeesAsync()
        {


            IList<Employee> employees = await cache?.TryGetAsync<IList<Employee>>("GetAllEmployees");


            if (employees == null)
            {
                IEnumerable<Employee> _employees = repo.GetAllWithProperty(e => e.City_obj);
                employees = (IList<Employee>) mapper.Map<IEnumerable<Employee>>(_employees);
                await cache.TrySetAsync("GetAllEmployees", employees);
            }

            var emps = (IList<EmployeeViewModel>)mapper.Map<IEnumerable<EmployeeViewModel>>(employees);

            logger.LogInformation("All employees requested by user {0}", user);

            return emps;
        }


        public async Task<IList<EmployeeDTO>> GetAllEmployeesWithoutCacheAsync()
        {
            IList<Employee> employees = (IList<Employee>)await repo.GetAllAsync();
            var emps = (IList<EmployeeDTO>)mapper.Map<IEnumerable<EmployeeDTO>>(employees);

            logger.LogInformation("All employees requested by Background service");

            return emps;
        }
    }
}

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
