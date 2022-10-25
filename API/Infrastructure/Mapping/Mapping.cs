using API.Models;
using API.Models.DTOs;
using API.Models.ViewModels;
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

            CreateMap<EmployeeDTO, EmployeeViewModel>();

            CreateMap<EmployeeViewModel, EmployeeDTO>();
            CreateMap<EmployeeViewModel, Employee>();

            CreateMap<Employee, EmployeeViewModel>();
            CreateMap<Employee,EmployeeViewModel>().ForMember(ev => ev.city_name,e => e.MapFrom(s => s.City_obj.name));
        }
    }
}
