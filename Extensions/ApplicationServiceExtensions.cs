using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using socialbackend.Data;
using socialbackend.interfaces;

namespace socialbackend.Extensions;

public static class ApplicationServiceExtensions
{

    public static IServiceCollection AddApplicationServices (this IServiceCollection services, IConfiguration config)
    {
        
        services.AddDbContext<DataContext>((options) =>
        {
            options.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });
        
        services.AddScoped<ITokenService, TokenService>();
        services.AddCors();
        
        return services;    
    }
}