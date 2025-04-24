using Microsoft.AspNetCore.Mvc;
using ArtGallery.Application.DTOs;
using ArtGallery.Application.Services;

namespace ArtGallery.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
	private readonly IUserService _userService;

	public AuthController(IUserService userService)
	{
		_userService = userService;
	}

	[HttpPost("register")]
	public async Task<IActionResult> Register(RegisterDto dto)
	{
		var result = await _userService.RegisterAsync(dto);
		return Ok(result);
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login(LoginDto dto)
	{
		var result = await _userService.LoginAsync(dto);
		if (result == null)
			return Unauthorized("Invalid credentials");

		return Ok(result);
	}
}
