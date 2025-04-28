using ArtGallery.Application.DependencyInjection;
using ArtGallery.Application.Services;
using ArtGallery.Infrastructure.DependencyInjection;
using ArtGallery.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.FileProviders;
using ArtGallery.API.Hubs;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// 1) Connection string: Connect to the ArtGalleryDb database
var connectionString = configuration.GetConnectionString("DefaultConnection")
		?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

// 2) EF Core: Add DbContext for AppDbContext using SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
		options.UseSqlServer(connectionString));

// 3) Core services: Register all application-level and infrastructure-level services
builder.Services.AddApplicationServices();          // registers ArtworkService, AuthService, etc.
builder.Services.AddInfrastructureServices(connectionString); // registers repositories, migrations, etc.

// 4) Explicit scoped registrations for user, auction & admin flows
builder.Services.AddScoped<IUserService, UserService>();     // user self-registration & login
builder.Services.AddScoped<IAuthService, AuthService>();     // full JWT auth flows
builder.Services.AddScoped<IAuctionService, AuctionService>(); // real-time bidding
builder.Services.AddScoped<IAdminService, AdminService>();   // approve/reject artists & artworks

// 5) Controllers: Add MVC controllers to handle API endpoints
builder.Services.AddControllers();

// 6) JWT Authentication: Configure JWT bearer scheme
var jwt = configuration.GetSection("Jwt");
var keyBytes = Encoding.UTF8.GetBytes(jwt["Key"]
		?? throw new InvalidOperationException("JWT Key is not configured."));
builder.Services
		.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		})
		.AddJwtBearer(options =>
		{
			options.RequireHttpsMetadata = false;
			options.SaveToken = true;
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
				ValidateIssuer = true,
				ValidIssuer = jwt["Issuer"],
				ValidateAudience = true,
				ValidAudience = jwt["Audience"],
				ValidateLifetime = true,
				ClockSkew = TimeSpan.Zero
			};
		});

// 7) Authorization: Policies for each role
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
	options.AddPolicy("ArtistOnly", policy => policy.RequireRole("Artist"));
	options.AddPolicy("BuyerOnly", policy => policy.RequireRole("Buyer"));
});

// 8) Swagger / OpenAPI: API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "ArtGallery API",
		Version = "v1",
		Description = "Manage users, artists, artworks & bids"
	});
});

// 9) SignalR: Real-time auction hub
builder.Services.AddSignalR();

var app = builder.Build();

// 10) Ensure uploads folder exists (for artwork image uploads)
var uploadsPath = Path.Combine(app.Environment.WebRootPath ?? "wwwroot", "uploads");
if (!Directory.Exists(uploadsPath))
	Directory.CreateDirectory(uploadsPath);

// 11) Middleware pipeline
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "ArtGallery API v1");
		c.RoutePrefix = string.Empty;
	});
}

app.UseHttpsRedirection();

// Serve static files (wwwroot & uploads folder)
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(uploadsPath),
	RequestPath = "/uploads"
});

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Map the real-time Auction hub
app.MapHub<AuctionHub>("/hubs/auction");

// Map API controllers
app.MapControllers();

app.Run();
