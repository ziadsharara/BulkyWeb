using Microsoft.AspNetCore.Mvc;
using ArtGallery.Application.DTOs;
using ArtGallery.Application.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ArtGallery.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _auth;
		private readonly ILogger<AuthController> _logger;

		public AuthController(IAuthService auth, ILogger<AuthController> logger)
		{
			_auth = auth;
			_logger = logger;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterDto dto)
		{
			if (dto == null)
			{
				_logger.LogWarning("Register request received with empty body.");
				return BadRequest(new { message = "Invalid registration data." });
			}

			try
			{
				var result = await _auth.RegisterAsync(dto);
				return Ok(result);
			}
			catch (InvalidOperationException ex)
			{
				_logger.LogError(ex, "Error during registration.");
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto dto)
		{
			if (dto == null)
			{
				_logger.LogWarning("Login request received with empty body.");
				return BadRequest(new { message = "Invalid login data." });
			}

			var result = await _auth.LoginAsync(dto);
			if (result == null)
			{
				_logger.LogWarning("Failed login attempt.");
				return Unauthorized(new { message = "Invalid credentials" });
			}

			_logger.LogInformation("User logged in successfully.");
			return Ok(result);  // This would usually return a JWT or some other login token
		}
	}
}
