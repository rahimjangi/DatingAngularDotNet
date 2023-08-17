using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        // Add services to the container.
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(config.GetConnectionString("default"));
        });
        services.AddScoped<ITokenService, TokenService>();


        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddCors();


        return services;
    }
}
