using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using ArtGallery.Application.DTOs;
using ArtGallery.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtGallery.API.Controllers
{
	// Base route for User-related API endpoints
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IAuthService _auth;

		// Constructor to inject IAuthService dependency
		public UserController(IAuthService auth) => _auth = auth;

		/// <summary>
		/// Registers a new user in the system.
		/// </summary>
		/// <param name="dto">The registration data (username, email, password).</param>
		/// <returns>An IActionResult indicating success or failure.</returns>
		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterDto dto)
		{
			try
			{
				// Attempt to register the user and return the user information
				var user = await _auth.RegisterAsync(dto);
				return Ok(user);
			}
			catch (InvalidOperationException ex)
			{
				// If registration fails, return a BadRequest with the error message
				return BadRequest(new { message = ex.Message });
			}
		}

		/// <summary>
		/// Logs in a user and returns a JWT token if credentials are valid.
		/// </summary>
		/// <param name="dto">The login data (email and password).</param>
		/// <returns>An IActionResult containing the JWT token if successful, or an Unauthorized response if failed.</returns>
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto dto)
		{
			try
			{
				// Attempt to authenticate the user and return the JWT token
				var token = await _auth.LoginAsync(dto);
				return Ok(new { token });
			}
			catch (UnauthorizedAccessException ex)
			{
				// If authentication fails, return Unauthorized with the error message
				return Unauthorized(new { message = ex.Message });
			}
		}

		/// <summary>
		/// Example protected endpoint that requires a valid JWT token for access.
		/// This endpoint returns the user ID from the token.
		/// </summary>
		/// <returns>An IActionResult containing the user ID if the token is valid.</returns>
		[Authorize]
		[HttpGet("me")]
		public ActionResult Me()
		{
			// Extract the "userId" claim we added in AuthService
			var userId = User.FindFirstValue("userId");

			if (string.IsNullOrEmpty(userId))
			{
				return Unauthorized(new { message = "User ID is missing in the token" });
			}

			return Ok(new { userId });
		}
	}
}
