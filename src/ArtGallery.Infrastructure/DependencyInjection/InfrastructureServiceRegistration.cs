namespace ArtGallery.Infrastructure.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ArtGallery.Infrastructure.Data;

public static class InfrastructureServiceRegistration
{
	public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
	{
		services.AddDbContext<AppDbContext>(options =>
				options.UseSqlServer(connectionString));

		return services;
	}
}

