using SoftServe_TestProject.Application.Services;
using SoftServe_TestProject.Data.Repositories;
using SoftServe_TestProject.Domain.Repositories;

namespace SoftServe_TestProject.API.ServicesConfiguration
{
    public static class ScopedServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ITeacherRepository, ITeacherRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<StudentService>();
            services.AddScoped<TeacherService>();
            services.AddScoped<CourseService>();

            return services;
        }
    }
}
