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

		// Constructor to inject AppDbContext for database access
		public UserService(AppDbContext db) => _db = db;

		// Registers a new user after hashing the password
		public async Task<UserDto> RegisterAsync(RegisterDto dto)
		{
			using var sha = SHA256.Create();
			var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

			// Create a new user entity
			var user = new Domain.Entities.User
			{
				Username = dto.Username,
				Email = dto.Email,
				PasswordHash = Convert.ToHexString(hash), // Store hashed password
				Role = dto.Role
			};

			// Save the new user to the database
			_db.Users.Add(user);
			await _db.SaveChangesAsync();

			// Return the UserDto with the user's information (without password)
			return new UserDto
			{
				Id = user.Id,
				Username = user.Username,
				Email = user.Email,
				Role = user.Role
			};
		}

		// Logs in an existing user by verifying email and password
		public async Task<UserDto?> LoginAsync(LoginDto dto)
		{
			// Find the user by email
			var user = await _db.Users
													 .FirstOrDefaultAsync(u => u.Email == dto.Email);
			if (user == null) // If user is not found, return null
				return null;

			// Hash the provided password and compare with the stored hash
			using var sha = SHA256.Create();
			var hash = Convert.ToHexString(
					sha.ComputeHash(Encoding.UTF8.GetBytes(dto.Password))
			);
			if (!hash.Equals(user.PasswordHash)) // If password doesn't match, return null
				return null;

			// Return UserDto with user data (excluding password)
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
