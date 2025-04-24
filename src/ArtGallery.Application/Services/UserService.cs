using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Application.Services;

using ArtGallery.Infrastructure.Data;
using ArtGallery.Domain.Entities;
using ArtGallery.Application.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

public class UserService : IUserService
{
	private readonly AppDbContext _db;
	public UserService(AppDbContext db) => _db = db;

	public async Task<UserDto> RegisterAsync(RegisterDto dto)
	{
		// hash password (SHA256 example)
		using var sha = SHA256.Create();
		var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));
		var user = new User
		{
			Username = dto.Username,
			Email = dto.Email,
			PasswordHash = Convert.ToHexString(hash),
			Role = dto.Role
		};
		_db.Users.Add(user);
		await _db.SaveChangesAsync();

		return new UserDto { Id = user.Id, Username = user.Username, Email = user.Email, Role = user.Role };
	}

	public async Task<UserDto?> LoginAsync(LoginDto dto)
	{
		var user = await _db.Users.SingleOrDefaultAsync(u => u.Email == dto.Email);
		if (user == null) return null;
		using var sha = SHA256.Create();
		var hash = Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)));
		if (!hash.Equals(user.PasswordHash)) return null;
		// create token logic here...
		return new UserDto { Id = user.Id, Username = user.Username, Email = user.Email, Role = user.Role };
	}
}