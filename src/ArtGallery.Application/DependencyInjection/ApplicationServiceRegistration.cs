using Microsoft.Extensions.DependencyInjection;
using ArtGallery.Application.Services;

namespace ArtGallery.Application.DependencyInjection
{
	public static class ApplicationServiceRegistration
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IArtworkService, ArtworkService>();
			services.AddScoped<IAuctionService, AuctionService>();
			return services;
		}
	}
}
