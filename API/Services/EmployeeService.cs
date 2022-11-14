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
        string? user;
        ISessionService sessionService;

        public EmployeeService(IGenericRepo<Employee> repo, IMapper mapper, ICacheManager cache, ILogger<EmployeeService> logger, ISessionService sessionService, ICityService cityService)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.cache = cache;
            this.logger = logger;
            this.sessionService = sessionService;
            this.cityService = cityService;
        }

        public async Task<ResponseModel> AddEmployeeAsync(EmployeeViewModel nEmp)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                EmployeeDTO newEmp = mapper.Map<EmployeeDTO>(nEmp);
                user = sessionService.GetString("user");

                var newEm = mapper.Map<Employee>(newEmp);
                newEm.city_id = cityService.GetCityByName(nEmp?.city_name).id;

                var emp = await repo.AddAsync(newEm);
                await repo.SaveChangesAsync();
                logger.LogInformation("Employee {0} added by {1}", newEmp.name, user);

                response.StatusCode = StatusCodes.Status201Created;
                response.Status = "Added";
                response.Message = "Employee added";
                response.Object = mapper.Map<EmployeeDTO>(emp);
                return response;
            }
            catch (Exception ex)
            {
                logger.LogInformation("Unable to add employee by user {0}", user);
                response.StatusCode = StatusCodes.Status500InternalServerError;
                response.Status = "Failed";
                response.Message = ex.Message;
                response.Object = nEmp;
                return response;
            }
        }

        public async Task<ResponseModel> GetAllEmployeesAsync()
        {
            ResponseModel response = new ResponseModel();

            IList<Employee> employees = await cache?.TryGetAsync<IList<Employee>>("GetAllEmployees");
            response.StatusCode = StatusCodes.Status304NotModified;


            if (employees == null)
            {
                IEnumerable<Employee> _employees = repo.GetAllWithProperty(e => e.City_obj);
                employees = (IList<Employee>)mapper.Map<IEnumerable<Employee>>(_employees);
                await cache.TrySetAsync("GetAllEmployees", employees);
                response.StatusCode = StatusCodes.Status200OK;
            }

            var emps = (IList<EmployeeViewModel>)mapper.Map<IEnumerable<EmployeeViewModel>>(employees);
            response.Object = emps;
            response.Message = "All employees requested by user";

            logger.LogInformation("All employees requested by user {0}", user);

            return response;
        }

        public async Task<IList<EmployeeDTO>> GetAllEmployeesWithoutCacheAsync()
        {
            IList<Employee> employees = (IList<Employee>)await repo.GetAllAsync();
            var emps = (IList<EmployeeDTO>)mapper.Map<IEnumerable<EmployeeDTO>>(employees);

            logger.LogInformation("All employees requested by Background service");

            return emps;
        }

        public ResponseModel GetEmployeeById(int id)
        {
           var emp = repo.Find(e => e.id == id).FirstOrDefault();
            if (emp != null)
            {
                return new ResponseModel()
                {
                    Object = mapper.Map<EmployeeDTO>(emp),
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

        public async Task<ResponseModel> UpdateEmployeeAsync(EmployeeViewModel nEmp)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                EmployeeDTO newEmp = mapper.Map<EmployeeDTO>(nEmp);
                user = sessionService?.GetString("user");
                var newEm = mapper.Map<Employee>(newEmp);
                newEm.city_id = cityService.GetCityByName(nEmp?.city_name).id;
                var emps = repo.Find(e => e.id == newEm.id);

                if (emps.Count() > 0)
                {
                    repo.Update(newEm);
                    logger.LogInformation("Employee {0} updated by {1}", newEmp.name, user);
                }
                else
                {
                    await repo.AddAsync(newEm);
                    logger.LogInformation("Update request. Employee {0} wasn't on records. Inserted by {1}", newEmp.name, user);

                }
                await repo.SaveChangesAsync();
                response.Object = mapper.Map<EmployeeDTO>(newEm);
                response.StatusCode = StatusCodes.Status202Accepted;
                response.Status = "Updated";
            }
            catch (Exception ex)
            {

                logger.LogInformation("Unable to update employee by user {0}", user);
                response.StatusCode = StatusCodes.Status500InternalServerError;
                response.Status = "Failed";
                response.Message = ex.Message;
                response.Object = nEmp;
                return response;
            }

            return response;
        }

        public async Task<ResponseModel> DeleteEmployeeAsync(EmployeeViewModel emp)
        {
            user = sessionService?.GetString("user");

            var _emp = mapper.Map<Employee>(emp);
            var result = repo.Find(e => e.id == _emp.id);

            if(result.Count() > 0)
            {
                repo.Delete(result.First());
                await repo.SaveChangesAsync();
                logger.LogInformation("Employee {0} deleted by {1}",_emp.name, user);
                return new ResponseModel() { StatusCode = StatusCodes.Status200OK, Message = "Deleted", Object = emp, Status = "Deleted"};
            }
            else
            {
                logger.LogInformation("Unable to delete employee {0} by {1}", _emp.name, user);
                return new ResponseModel() { StatusCode = StatusCodes.Status400BadRequest, Message = "Not Deleted", Object = emp, Status = "Unable to Delete" }; ;
            }
        }
    }
}

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
