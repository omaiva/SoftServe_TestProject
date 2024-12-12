using Microsoft.Extensions.DependencyInjection;
using SoftServe_TestProject.Application.MappingProfiles;

namespace SoftServe_TestProject.Application.ExtensionServices
{
    public static class AddMappingProfilesService
    {
        public static IServiceCollection AddMappingProfilesOfRequests(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(CourseProfile).Assembly);
            services.AddAutoMapper(typeof(StudentProfile).Assembly);
            services.AddAutoMapper(typeof(TeacherProfile).Assembly);

            return services;
        }
    }
}
