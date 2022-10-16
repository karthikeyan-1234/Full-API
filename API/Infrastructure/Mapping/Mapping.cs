using API.Models;
using API.Models.DTOs;
using AutoMapper;

namespace API.Mapping
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CityDTO, City>();
            CreateMap<City, CityDTO>();

            CreateMap<EmployeeDTO, Employee>();
            CreateMap<Employee, EmployeeDTO>();
        }
    }
}
