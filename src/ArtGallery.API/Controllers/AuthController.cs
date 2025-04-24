using Microsoft.AspNetCore.Mvc;
using ArtGallery.Application.DTOs;
using ArtGallery.Application.Services;

namespace ArtGallery.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _auth;

		public AuthController(IAuthService auth) => _auth = auth;

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterDto dto)
		{
			try
			{
				var result = await _auth.RegisterAsync(dto);
				return Ok(result);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto dto)
		{
			var result = await _auth.LoginAsync(dto);
			if (result == null)
				return Unauthorized("Invalid credentials");

			Console.WriteLine(result);

			return Ok(result);
		}

	}
}
