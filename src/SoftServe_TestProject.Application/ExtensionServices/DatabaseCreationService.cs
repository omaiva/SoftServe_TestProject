using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SoftServe_TestProject.Data;
using Microsoft.EntityFrameworkCore;

namespace SoftServe_TestProject.Application.ExtensionServices
{
    public static class DatabaseCreationService
    {
        public static async Task DatabaseEnsureCreated(this IApplicationBuilder applicationBuilder)
        {
            using (var scope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ProjectContext>();
                var database = dbContext.Database;
                
                await database.EnsureCreatedAsync();
                await database.MigrateAsync();
            }
        }
    }
}
