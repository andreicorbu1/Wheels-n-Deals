using DealershipAPI.Repository;
using DealershipAPI.Service;
using System.Reflection;

namespace DealershipAPI.Settings;

public static class Dependencies
{
	public static void Inject(WebApplicationBuilder applicationBuilder)
	{
		applicationBuilder.Services.AddControllers();
		applicationBuilder.Services.AddSwaggerGen(c =>
		{
			var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
			var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
			c.IncludeXmlComments(xmlPath);
		});
		AddRepositories(applicationBuilder.Services);
		AddServices(applicationBuilder.Services);
	}

	private static void AddRepositories(IServiceCollection services)
	{
		services.AddScoped<UserRepository>();
	}

	private static void AddServices(IServiceCollection services)
	{
		services.AddScoped<AppDbContext>();
		services.AddScoped<UserService>();
	}
}
