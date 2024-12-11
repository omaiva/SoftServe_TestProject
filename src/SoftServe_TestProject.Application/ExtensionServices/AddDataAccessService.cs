using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoftServe_TestProject.Data;

namespace SoftServe_TestProject.Application.ExtensionServices
{
    public static class DataAccessService
    {
        public static IServiceCollection AddDataAccessService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ProjectContext>(options =>
            {
                options.UseMySql(
                        configuration.GetConnectionString("ProjectConnection"),
                        new MySqlServerVersion(new Version(10, 4, 32)),
                        mySqlOptions => mySqlOptions.EnableRetryOnFailure()
                    );
            });

            return services;
        }
    }
}
