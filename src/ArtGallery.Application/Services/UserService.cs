using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ArtGallery.Application.DTOs;
using ArtGallery.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Application.Services
{
	public class UserService : IUserService
	{
		private readonly AppDbContext _db;
		public UserService(AppDbContext db) => _db = db;

		public async Task<UserDto> RegisterAsync(RegisterDto dto)
		{
			using var sha = SHA256.Create();
			var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

			var user = new Domain.Entities.User
			{
				Username = dto.Username,
				Email = dto.Email,
				PasswordHash = Convert.ToHexString(hash),
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

		public async Task<UserDto?> LoginAsync(LoginDto dto)
		{
			var user = await _db.Users
													.FirstOrDefaultAsync(u => u.Email == dto.Email);
			if (user == null)
				return null;

			using var sha = SHA256.Create();
			var hash = Convert.ToHexString(
					sha.ComputeHash(Encoding.UTF8.GetBytes(dto.Password))
			);
			if (!hash.Equals(user.PasswordHash))
				return null;

			return new UserDto
			{
				Id = user.Id,
				Username = user.Username,
				Email = user.Email,
				Role = user.Role
			};
		}
	}
}
