using TaskRunner.LongTask;

namespace TaskRunner.Services
{

    /**
     * The purpose of this class is to start an multiple backgroundservices, one for each big task.
     * Each BackgroundService has its own Dependency inejection container.
     **/
    public class ConsumeBigTask : BackgroundService
    {
        private readonly ILogger<ConsumeBigTask> _logger;

        public IServiceProvider Services { get; }
        public ConsumeBigTask(IServiceProvider services, ILogger<ConsumeBigTask> logger)
        {
            Services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Consume Scoped Service Hosted Service running.");
        }
        public async Task RunBigTasks(dynamic anonymousParams)
        {

            _logger.LogInformation("RunBigTasks");

            using (var scope = Services.CreateScope())
            {
                var BigTask =
                    scope.ServiceProvider
                        .GetRequiredService<IBigTask>();

                await BigTask.RunAsync(anonymousParams);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Consume Scoped Service Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
