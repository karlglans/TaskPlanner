namespace TaskRunner.Services
{
    public static class ServiceProviderExtensions
    {
        public static ConsumeBigTask GetHostedService<ConsumeBigTask>
            (this IServiceProvider serviceProvider) =>
            serviceProvider
                .GetServices<IHostedService>()
                .OfType<ConsumeBigTask>()
                .FirstOrDefault();
    }

}
