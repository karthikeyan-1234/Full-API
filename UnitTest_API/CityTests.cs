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
        MapperConfiguration mapper_config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new Mapping());
        });
        ICityService cityService;
        ICacheManager cacheManager;
        IMapper mapper;
        ILogger<CacheManager> logger;
        ILogger<CityService> cityLogger;
        Mock<IMediator> mockMediator;
        Mock<ISessionService> mockSessionService;

        public CityTests()
        {
            mockCityRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(Cities);
            mockCityRepo.Setup(x => x.AddAsync(It.IsAny<City>())).ReturnsAsync((City C) =>
            {
                C.id = Cities.Count + 1;
                Cities.Add(C);
                return C;   
            });

            mapper = mapper_config.CreateMapper();
            logger = new NullLogger<CacheManager>();
            cityLogger = new NullLogger<CityService>();

            mockSessionService = new Mock<ISessionService>();
            mockSessionService.Setup(s => s.GetString("user")).Returns("Arjun");

            mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(It.IsAny<GetAllCitiesQuery>(),default)).Returns(Task.FromResult(CityDTOs));

            Mock<IDistributedCache> mockCache = new Mock<IDistributedCache>();
            cacheManager = new CacheManager(mockCache.Object, mockSessionService.Object, logger);

            cityService = new CityService(mockCityRepo.Object, mapper, cityLogger, cacheManager, mockMediator.Object,mockSessionService.Object);

        }

        [Fact]
        void Should_Return_All_Cities()
        {
            var cities = cityService.GetAllCitiesAsync();
            Assert.IsType<Task<IList<CityDTO>>>(cities);
        }

        [Fact]
        void Should_Session_Service_Return_Session_Data()
        {
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
        }


    }


  
}