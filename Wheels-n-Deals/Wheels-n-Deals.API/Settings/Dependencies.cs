using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;
using Wheels_n_Deals.API.DataLayer;
using Wheels_n_Deals.API.DataLayer.Repositories;
using Wheels_n_Deals.API.Services;

namespace Wheels_n_Deals.API.Settings;

public class Dependencies
{
    public static void Inject(WebApplicationBuilder applicationBuilder)
    {
        applicationBuilder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(applicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
        });
        applicationBuilder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,

                ValidIssuer = "Backend",
                ValidAudience = "Frontend",
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(applicationBuilder.Configuration["JWT:SecurityKey"]))
            };
        });
        applicationBuilder.Services.AddControllers();
        applicationBuilder.Services.AddSwaggerGen(c =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });
        applicationBuilder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        AddRepositories(applicationBuilder.Services);
        AddServices(applicationBuilder.Services);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<UserRepository>();
        services.AddScoped<VehicleRepository>();
        services.AddScoped<FeaturesRepository>();
        services.AddScoped<UnitOfWork>();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<AuthorizationService>();
        services.AddScoped<VehicleService>();
        services.AddScoped<UserService>();
    }
}