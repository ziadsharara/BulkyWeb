using ArtGallery.Application.DTOs;
using ArtGallery.Domain.Entities;
using ArtGallery.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Application.Services
{
	public class AuthService : IAuthService
	{
		private readonly AppDbContext _db;
		private readonly IConfiguration _config;

		public AuthService(AppDbContext db, IConfiguration config)
		{
			_db = db;
			_config = config;
		}

		public async Task<UserDto> RegisterAsync(RegisterDto dto)
		{
			if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
				throw new InvalidOperationException("Email already in use.");

			using var sha = SHA256.Create();
			var hash = Convert.ToHexString(
					sha.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)));

			var user = new User
			{
				Username = dto.Username,
				Email = dto.Email,
				PasswordHash = hash,
				Role = dto.Role
			};

			_db.Users.Add(user);
			await _db.SaveChangesAsync();

			return new UserDto
			{
				Id = user.Id,
				Username = user.Username,
				Email = user.Email,
				Role = user.Role
			};
		}

		public async Task<string> LoginAsync(LoginDto dto)
		{
			var user = await _db.Users
					.SingleOrDefaultAsync(u => u.Email == dto.Email);
			if (user == null)
				throw new UnauthorizedAccessException("Invalid credentials.");

			using var sha = SHA256.Create();
			var hash = Convert.ToHexString(
					sha.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)));

			if (user.PasswordHash != hash)
				throw new UnauthorizedAccessException("Invalid credentials.");

			// Build JWT
			var jwt = _config.GetSection("Jwt");
			var key = Encoding.UTF8.GetBytes(jwt["Key"]!);
			var claims = new[] {
								new Claim(JwtRegisteredClaimNames.Sub,   user.Id.ToString()),
								new Claim(JwtRegisteredClaimNames.Email, user.Email),
								new Claim("role",                        user.Role)
						};
			var creds = new SigningCredentials(
												new SymmetricSecurityKey(key),
												SecurityAlgorithms.HmacSha256);
			var expires = DateTime.UtcNow
										.AddMinutes(double.Parse(jwt["DurationMinutes"]!));

			var token = new JwtSecurityToken(
					issuer: jwt["Issuer"],
					audience: jwt["Audience"],
					claims: claims,
					expires: expires,
					signingCredentials: creds
			);

			return new JwtSecurityTokenHandler()
						 .WriteToken(token);
		}
	}
}
