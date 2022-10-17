using API.Services;
using Microsoft.AspNetCore.SignalR.Client;

namespace API.Infrastructure.BackGroundTasks
{
    public class APIBackGroundService : BackgroundService
    {
        ILogger<APIBackGroundService> logger;
        IServiceProvider _serviceProvider;
        HubConnection connection;

        public APIBackGroundService(ILogger<APIBackGroundService> logger,IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this._serviceProvider = serviceProvider;
            connection = new HubConnectionBuilder().WithUrl("https://localhost:7101/MessageHub").Build();

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await connection.StartAsync();
            connection.InvokeAsync("JoinGroup", "MyGroup").Wait();
            logger.LogInformation("Joined the MyGroup");

            while (!stoppingToken.IsCancellationRequested)
            {
                Thread.Sleep(3000);

                using (IServiceScope scope = _serviceProvider.CreateScope())
                {
                    IEmployeeService scopedProcessingService =
                        scope.ServiceProvider.GetRequiredService<IEmployeeService>();

                    var users = await scopedProcessingService.GetAllEmployeesWithoutCacheAsync();   //Use the without cache option as the httpContextSession
                                                                                                    //can't be accessed from BackgroundServices
                    logger.LogInformation("No of employees {0}", users.Count());
                    string? message = "No of employees " + users.Count();
                    await connection.InvokeAsync("SendMessageToGroup", "MyGroup", "PLC", message);
                }

            }
        }
    }
}
