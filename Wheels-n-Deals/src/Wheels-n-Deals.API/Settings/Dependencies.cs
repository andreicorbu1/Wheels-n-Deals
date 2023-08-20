using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;
using Wheels_n_Deals.API.DataLayer;
using Wheels_n_Deals.API.DataLayer.Interfaces;
using Wheels_n_Deals.API.DataLayer.Repositories;
using Wheels_n_Deals.API.Services;
using Wheels_n_Deals.API.Services.Interfaces;

namespace Wheels_n_Deals.API.Settings;

public class Dependencies
{
    public static void Inject(WebApplicationBuilder app)
    {
        app.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(app.Configuration.GetConnectionString("DefaultConnection"),
                o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
        });
        app.Services.AddAuthentication(options =>
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
                        Encoding.UTF8.GetBytes(app.Configuration["JWT:SecurityKey"] ?? "413F4428472B4B6250655368566D5970337336763979244226452948404D6351"))
            };
        });
        app.Services.AddControllers();
        app.Services.AddSwaggerGen(c =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });
        app.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        app.Services.AddCors(options =>
        {
            options.AddPolicy("Access-Control-Allow-Origin",
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
        });
        AddRepositories(app.Services);
        AddServices(app.Services);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFeatureRepository, FeatureRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
        services.AddScoped<IImageRepository, ImageRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<IAnnouncementService, AnnouncementService>();
    }
}