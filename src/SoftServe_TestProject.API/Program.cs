using SoftServe_TestProject.API.Middleware;
using SoftServe_TestProject.API.ServicesConfiguration;
using SoftServe_TestProject.Application.ExtensionServices;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "ProjectAPI", 
        Version = "v1",
        Description = "A .NET Core WEB API project that performs CRUD operations on Course, Student and Teacher entities."
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddServices();
builder.Services.AddValidators();
builder.Services.AddDataAccessService(builder.Configuration);
builder.Services.AddMappingProfiles();
builder.Services.AddMappingProfilesOfRequests();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ProjectApi V1");
    });

    await app.DatabaseEnsureCreated();
}

app.UseSerilogRequestLogging();
app.UseMiddleware<RequestLogContextMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
