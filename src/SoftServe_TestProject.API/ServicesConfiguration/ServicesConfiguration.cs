using FluentValidation;
using Microsoft.OpenApi.Models;
using SoftServe_TestProject.API.MappingProfiles;
using SoftServe_TestProject.API.Validators;
using SoftServe_TestProject.Application.Interfaces;
using SoftServe_TestProject.Application.Services;
using SoftServe_TestProject.Data.Repositories;
using SoftServe_TestProject.Domain.Interfaces;
using System.Reflection;

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
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<ICourseService, CourseService>();    

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

        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ProjectAPI",
                    Version = "v1",
                    Description = "A .NET Core WEB API project that performs CRUD operations on Course, Student and Teacher entities."
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                options.IncludeXmlComments(xmlPath);
            });

            return services;
        }
    }
}
