using FluentValidation;
using SoftServe_TestProject.API.MappingProfiles;
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

            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<StudentDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<TeacherDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<CourseDTOValidator>();

            return services;
        }

        public static IServiceCollection AddMappingProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(CourseProfile).Assembly);
            services.AddAutoMapper(typeof(StudentProfile).Assembly);
            services.AddAutoMapper(typeof(TeacherProfile).Assembly);

            return services;
        }
    }
}
