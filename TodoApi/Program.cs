using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TodoApi.Models;
using TodoApi.Services;
using UsersApi.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddDbContext<WebAppContext>(opt =>
    opt.UseInMemoryDatabase("UsersDb")
       .UseLazyLoadingProxies()
    );

builder.Services.AddTransient<IUserService, UsersService>();

builder.AddServiceDefaults();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1.0",
        Title = "Users API",
        Description = "An ASP.NET Core Web API for managing users"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapDefaultEndpoints();

using (var scope = app.Services.CreateScope())
{
    StartUpManager.APIStartUp(scope.ServiceProvider);
}

app.Run();
