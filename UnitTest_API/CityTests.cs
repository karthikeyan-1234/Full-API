using API.Controllers;
using API.DAL.Queries;
using API.Infrastructure.Caching;
using API.Mapping;
using API.Models;
using API.Models.DTOs;
using API.Repositories;
using API.Services;
using AutoMapper;
using Castle.Core.Logging;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace UnitTest_API
{
    public class CityTests
    {
        List<City> Cities = new List<City>()
        {
            new City() { id = 1, name = "Chennai" },
            new City() { id = 2, name = "Coimbatore"}
        };

        IList<CityDTO> CityDTOs = new List<CityDTO>()
        {
            new CityDTO() { id = 1, name = "Chennai" },
            new CityDTO() { id = 2, name = "Coimbatore"}
        };


        Mock<IGenericRepo<City>> mockCityRepo = new Mock<IGenericRepo<City>>();
        ICityService cityService;
        ICacheManager cacheManager;
        IMapper mapper;
        ILogger<CacheManager> logger;
        ILogger<CityService> cityLogger;
        Mock<IMediator> mockMediator;
        Mock<ISessionService> mockSessionService;

        public CityTests()
        {
            //Repo setup

            mockCityRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(Cities);
            mockCityRepo.Setup(x => x.AddAsync(It.IsAny<City>())).ReturnsAsync((City C) =>
            {
                C.id = Cities.Count + 1;
                Cities.Add(C);
                return C;   
            });

            //Auto Mapper setup

            MapperConfiguration mapper_config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new Mapping());
            });
            mapper = mapper_config.CreateMapper();


            //Logger setup

            logger = new NullLogger<CacheManager>();
            cityLogger = new NullLogger<CityService>();


            //Session setup

            mockSessionService = new Mock<ISessionService>();
            mockSessionService.Setup(s => s.GetString("user")).Returns("Arjun");


            //Mediator setup

            mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(It.IsAny<GetAllCitiesQuery>(),default)).Returns(Task.FromResult(CityDTOs));


            //Cache setup

            Mock<IDistributedCache> mockCache = new Mock<IDistributedCache>();
            cacheManager = new CacheManager(mockCache.Object, mockSessionService.Object, logger);


            //Initialize services

            cityService = new CityService(mockCityRepo.Object, mapper, cityLogger, cacheManager, mockMediator.Object,mockSessionService.Object);

        }

        [Fact]
        void Should_Return_All_Cities()
        {
            var cities = cityService.GetAllCitiesAsync();
            Assert.IsType<Task<IList<CityDTO>>>(cities);
        }

        [Fact]
        void Should_Add_City()
        {
            CityDTO newCity = new CityDTO()
            {
                id = 3,
                name = "DummyCity",
                TenantId = "1"
            };

            Task<ResponseModel> res = cityService.AddCityAsync(newCity);

            Assert.Equivalent(newCity, res.Result.Object);
            res.Result.Object.Should().BeOfType<CityDTO>();
            var city = res.Result.Object as CityDTO;
            city.id.Should().Be(3);
        }

    }


  
}