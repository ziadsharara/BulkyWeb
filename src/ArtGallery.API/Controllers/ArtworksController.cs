using ArtGallery.Application.DTOs;
using ArtGallery.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using ArtGallery.Domain.Entities;

namespace ArtGallery.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ArtworksController : ControllerBase
	{
		private readonly IArtworkService _service;
		private static readonly string[] _allowedExt = new[] { ".jpg", ".jpeg", ".png", ".gif" };

		public ArtworksController(IArtworkService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll(
				[FromQuery] string? category,
				[FromQuery] string? tag,
				[FromQuery] string sortBy = "date",
				[FromQuery] string sortDirection = "desc",
				[FromQuery] int pageNumber = 1,
				[FromQuery] int pageSize = 10
		)
		{
			var paged = await _service.GetAllAsync(category, tag, sortBy, sortDirection, pageNumber, pageSize);
			return Ok(paged);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var artwork = await _service.GetByIdAsync(id);
			if (artwork == null) return NotFound("Artwork not found.");
			return Ok(artwork);
		}

		[HttpPost("upload-image")]
		[Authorize]
		public async Task<IActionResult> UploadImage(IFormFile file)
		{
			if (file == null || file.Length == 0)
				return BadRequest("No file uploaded.");

			var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
			if (!_allowedExt.Contains(ext))
				return BadRequest("Only image files are allowed.");

			var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
			if (!Directory.Exists(uploads))
				Directory.CreateDirectory(uploads);

			var name = $"{Guid.NewGuid()}{ext}";
			var path = Path.Combine(uploads, name);
			using var stream = new FileStream(path, FileMode.Create);
			await file.CopyToAsync(stream);

			return Ok(new { imageUrl = $"/uploads/{name}" });
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Create([FromForm] CreateArtworkDto dto, IFormFile? file)
		{
			var userId = GetUserIdFromToken();
			if (userId == null) return Unauthorized("Invalid or missing token.");

			// رفع الصورة لو موجودة
			if (file != null && file.Length > 0)
			{
				var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
				if (!_allowedExt.Contains(ext))
					return BadRequest("Only image files are allowed.");

				var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
				if (!Directory.Exists(uploads))
					Directory.CreateDirectory(uploads);

				var name = $"{Guid.NewGuid()}{ext}";
				var path = Path.Combine(uploads, name);
				using var stream = new FileStream(path, FileMode.Create);
				await file.CopyToAsync(stream);

				dto.ImageUrl = $"/uploads/{name}";
			}

			var created = await _service.CreateAsync(dto, userId.Value);
			return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
		}

		[HttpPut("{id}")]
		[Authorize]
		public async Task<IActionResult> Update(int id, [FromBody] ArtworkDto dto)
		{
			var updated = await _service.UpdateAsync(id, dto);
			if (!updated) return NotFound("Artwork not found.");
			return NoContent();
		}

		[HttpDelete("{id}")]
		[Authorize]
		public async Task<IActionResult> Delete(int id)
		{
			var deleted = await _service.DeleteAsync(id);
			if (!deleted) return NotFound("Artwork not found.");
			return NoContent();
		}

		// ✅ Helper: Extract UserId from JWT Token
		private int? GetUserIdFromToken()
		{
			var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
			return claim != null && int.TryParse(claim.Value, out var id) ? id : null;
		}
	}
}
