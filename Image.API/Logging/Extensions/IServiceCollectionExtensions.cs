using Microsoft.Extensions.DependencyInjection;

namespace Image.Logging.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddExceptionLogging(this IServiceCollection services)
        {
            services.ConfigureOptions<ExceptionLogOptions>();
        }
    }
}