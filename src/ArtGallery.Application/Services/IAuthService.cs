using ArtGallery.Application.DTOs;
using ArtGallery.Domain.Entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ArtGallery.Application.Services
{
	// Interface for authentication service
	public interface IAuthService
	{
		/// <summary>
		/// Registers a new user and returns an AuthResponseDto containing the user info and a JWT token.
		/// </summary>
		/// <param name="dto">The registration data (username, email, password).</param>
		/// <returns>An AuthResponseDto containing the user information and JWT token.</returns>
		Task<AuthResponseDto> RegisterAsync(RegisterDto dto);

		/// <summary>
		/// Logs in an existing user and returns an AuthResponseDto containing the user info and a JWT token.
		/// </summary>
		/// <param name="dto">The login data (email and password).</param>
		/// <returns>An AuthResponseDto containing the user information and JWT token.</returns>
		Task<AuthResponseDto> LoginAsync(LoginDto dto);
	}
}
