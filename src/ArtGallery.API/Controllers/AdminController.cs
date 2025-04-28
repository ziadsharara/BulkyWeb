using System.Collections.Generic;
using System.Threading.Tasks;
using ArtGallery.Application.DTOs;
using ArtGallery.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtGallery.API.Controllers
{
	/// <summary>
	///     AdminController allows administrators to manage pending artist registrations
	///     and pending artwork submissions (approve or reject).
	/// </summary>
	[ApiController]
	[Route("api/[controller]")]
	[Authorize(Roles = "Admin")]
	public class AdminController : ControllerBase
	{
		private readonly IAdminService _adminService;

		/// <summary>
		///     Injects the admin service to handle business logic.
		/// </summary>
		public AdminController(IAdminService adminService)
		{
			_adminService = adminService;
		}

		// ───────────────────────────────────────────────────────────────────────────────
		// Artist management
		// ───────────────────────────────────────────────────────────────────────────────

		/// <summary>
		///     Gets all newly registered artists awaiting approval.
		/// </summary>
		[HttpGet("artists/pending")]
		public async Task<ActionResult<IEnumerable<UserDto>>> GetPendingArtists()
		{
			var pending = await _adminService.GetPendingArtistsAsync();
			return Ok(pending);
		}

		/// <summary>
		///     Approves a pending artist account, enabling them to post artworks.
		/// </summary>
		/// <param name="artistId">ID of the artist to approve.</param>
		[HttpPost("artists/{artistId}/approve")]
		public async Task<IActionResult> ApproveArtist(int artistId)
		{
			var success = await _adminService.ApproveArtistAsync(artistId);
			if (!success) return NotFound(new { message = $"Artist {artistId} not found or already processed." });
			return NoContent();
		}

		/// <summary>
		///     Rejects a pending artist account registration.
		/// </summary>
		/// <param name="artistId">ID of the artist to reject.</param>
		[HttpPost("artists/{artistId}/reject")]
		public async Task<IActionResult> RejectArtist(int artistId)
		{
			var success = await _adminService.RejectArtistAsync(artistId);
			if (!success) return NotFound(new { message = $"Artist {artistId} not found or already processed." });
			return NoContent();
		}

		// ───────────────────────────────────────────────────────────────────────────────
		// Artwork management
		// ───────────────────────────────────────────────────────────────────────────────

		/// <summary>
		///     Gets all newly submitted artworks awaiting approval.
		/// </summary>
		[HttpGet("artworks/pending")]
		public async Task<ActionResult<IEnumerable<ArtworkDto>>> GetPendingArtworks()
		{
			var pending = await _adminService.GetPendingArtworksAsync();
			return Ok(pending);
		}

		/// <summary>
		///     Approves a pending artwork post, making it visible to buyers.
		/// </summary>
		/// <param name="artworkId">ID of the artwork to approve.</param>
		[HttpPost("artworks/{artworkId}/approve")]
		public async Task<IActionResult> ApproveArtwork(int artworkId)
		{
			var success = await _adminService.ApproveArtworkAsync(artworkId);
			if (!success) return NotFound(new { message = $"Artwork {artworkId} not found or already processed." });
			return NoContent();
		}

		/// <summary>
		///     Rejects a pending artwork post, preventing it from being displayed.
		/// </summary>
		/// <param name="artworkId">ID of the artwork to reject.</param>
		[HttpPost("artworks/{artworkId}/reject")]
		public async Task<IActionResult> RejectArtwork(int artworkId)
		{
			var success = await _adminService.RejectArtworkAsync(artworkId);
			if (!success) return NotFound(new { message = $"Artwork {artworkId} not found or already processed." });
			return NoContent();
		}
	}
}
