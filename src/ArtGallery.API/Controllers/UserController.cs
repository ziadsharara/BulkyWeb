using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using ArtGallery.Application.DTOs;
using ArtGallery.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtGallery.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IAuthService _auth;
		public UserController(IAuthService auth) => _auth = auth;

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterDto dto)
		{
			try
			{
				var user = await _auth.RegisterAsync(dto);
				return Ok(user);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto dto)
		{
			try
			{
				var token = await _auth.LoginAsync(dto);
				return Ok(new { token });
			}
			catch (UnauthorizedAccessException ex)
			{
				return Unauthorized(new { message = ex.Message });
			}
		}

		/// <summary>
		/// Example protected endpoint.
		/// </summary>
		[Authorize]
		[HttpGet("me")]
		public IActionResult Me()
		{
			// The sub claim holds the user ID
			var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
			return Ok(new { userId });
		}
	}
}
