using ArtGallery.Application.DTOs;
using ArtGallery.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ArtGallery.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ArtworksController : ControllerBase
	{
		private readonly IArtworkService _service;
		private readonly string[] _allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

		public ArtworksController(IArtworkService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var artworks = await _service.GetAllAsync();
			return Ok(artworks);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var artwork = await _service.GetByIdAsync(id);
			if (artwork == null) return NotFound();
			return Ok(artwork);
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] ArtworkDto dto)
		{
			var userId = GetUserId();
			if (userId == null) return Unauthorized(new { message = "Invalid user ID in token." });

			dto.ArtistId = userId.Value;
			var created = await _service.CreateAsync(dto);
			return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
		}

		[Authorize]
		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] ArtworkDto dto)
		{
			var userId = GetUserId();
			if (userId == null) return Unauthorized(new { message = "Invalid user ID in token." });

			dto.ArtistId = userId.Value;
			var updated = await _service.UpdateAsync(id, dto);
			if (!updated) return NotFound();
			return NoContent();
		}

		[Authorize]
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var deleted = await _service.DeleteAsync(id);
			if (!deleted) return NotFound();
			return NoContent();
		}

		[Authorize]
		[HttpPost("upload-image")]
		public async Task<IActionResult> UploadImage(IFormFile file)
		{
			if (file == null || file.Length == 0)
				return BadRequest("No file uploaded.");

			var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
			if (!_allowedImageExtensions.Contains(extension))
				return BadRequest("Only image files (jpg, png, gif) are allowed.");

			var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
			if (!Directory.Exists(uploadsFolder))
				Directory.CreateDirectory(uploadsFolder);

			var uniqueFileName = $"{Guid.NewGuid()}{extension}";
			var filePath = Path.Combine(uploadsFolder, uniqueFileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			var imageUrl = $"/uploads/{uniqueFileName}";
			return Ok(new { imageUrl });
		}

		private int? GetUserId()
		{
			var userIdStr = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
			return int.TryParse(userIdStr, out var userId) ? userId : null;
		}
	}
}
