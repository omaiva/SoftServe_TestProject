using SoftServe_TestProject.API.Middleware;
using SoftServe_TestProject.API.ServicesConfiguration;
using SoftServe_TestProject.Application.ExtensionServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddServices();
builder.Services.AddValidators();
builder.Services.AddDataAccessService(builder.Configuration);
builder.Services.AddMappingProfiles();
builder.Services.AddMappingProfilesOfRequests();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.DatabaseEnsureCreated();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
