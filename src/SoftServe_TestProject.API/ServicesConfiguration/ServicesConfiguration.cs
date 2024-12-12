using FluentValidation;
using SoftServe_TestProject.API.Validators;
using SoftServe_TestProject.Application.Services;
using SoftServe_TestProject.Data.Repositories;
using SoftServe_TestProject.Domain.Interfaces;

namespace SoftServe_TestProject.API.ServicesConfiguration
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ITeacherRepository, TeacherRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<StudentService>();
            services.AddScoped<TeacherService>();
            services.AddScoped<CourseService>();    

            services.AddValidatorsFromAssemblyContaining<StudentDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<TeacherDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<CourseDTOValidator>();

            return services;
        }
    }
}
