using API.Services;

namespace API.Infrastructure.BackGroundTasks
{
    public class APIBackGroundService : BackgroundService
    {
        ILogger<APIBackGroundService> logger;
        private readonly IServiceProvider _serviceProvider;

        public APIBackGroundService(ILogger<APIBackGroundService> logger,IServiceProvider serviceProvider)
        {
            this.logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                Thread.Sleep(3000);

                using (IServiceScope scope = _serviceProvider.CreateScope())
                {
                    IEmployeeService scopedProcessingService =
                        scope.ServiceProvider.GetRequiredService<IEmployeeService>();

                    var users = await scopedProcessingService.GetAllEmployeesWithoutCacheAsync();   //Use the without cache option as the httpContextSession
                                                                                                    //can't be accessed from BackgroundServices
                    logger.LogInformation("No of employees {0}", users.Count());
                }

            }
        }
    }
}
