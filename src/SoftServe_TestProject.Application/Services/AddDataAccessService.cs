using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoftServe_TestProject.Data;

namespace SoftServe_TestProject.Application.Services
{
    public static class DataAccessService
    {
        public static IServiceCollection AddDataAccessService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ProjectContext>();

            return services;
        }
    }
}
