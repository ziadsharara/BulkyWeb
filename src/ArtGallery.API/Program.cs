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

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// 1) Connection string
var connectionString = configuration.GetConnectionString("DefaultConnection")
	?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

// 2) EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(connectionString));

// 3) Application & Infrastructure services
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(connectionString);

// 4) AuthService
builder.Services.AddScoped<IAuthService, AuthService>();

// 5) Controllers
builder.Services.AddControllers();

// 6) JWT Authentication
var jwt = configuration.GetSection("Jwt");
builder.Services
	.AddAuthentication(options =>
	{
		options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	})
	.AddJwtBearer(options =>
	{
		options.RequireHttpsMetadata = true;
		options.SaveToken = true;
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidIssuer = jwt["Issuer"],
			ValidateAudience = true,
			ValidAudience = jwt["Audience"],
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!)),
			ValidateLifetime = true,
			ClockSkew = TimeSpan.Zero
		};
	});

// 7) Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "ArtGallery API",
		Version = "v1",
		Description = "Manage artworks, bids and users"
	});
});

var app = builder.Build();

// 8) Ensure wwwroot/uploads exists
var uploadsPath = Path.Combine(app.Environment.WebRootPath ?? "wwwroot", "uploads");
if (!Directory.Exists(uploadsPath))
{
	Directory.CreateDirectory(uploadsPath);
}

// 9) Middleware pipeline
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

// Serve static files from wwwroot
app.UseStaticFiles();

// Serve files from wwwroot/uploads
app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(uploadsPath),
	RequestPath = "/uploads"
});

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
