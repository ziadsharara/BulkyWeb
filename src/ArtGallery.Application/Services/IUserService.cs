using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Application.Services;

using ArtGallery.Application.DTOs;

// Interface for user-related services such as registration and login
public interface IUserService
{
	// Registers a new user and returns a UserDto containing user data
	Task<UserDto> RegisterAsync(RegisterDto dto);

	// Logs in an existing user and returns UserDto if credentials are valid; returns null if invalid
	Task<UserDto?> LoginAsync(LoginDto dto);
}
