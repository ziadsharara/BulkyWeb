using ArtGallery.Application.DTOs;
using ArtGallery.Domain.Entities;
using ArtGallery.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtGallery.Application.Services
{
	/// <summary>
	///     Implementation of IAdminService, handling approval/rejection workflows.
	/// </summary>
	public class AdminService : IAdminService
	{
		private readonly AppDbContext _db;

		/// <summary>
		///     Injects the application's DbContext for data access.
		/// </summary>
		public AdminService(AppDbContext db)
		{
			_db = db;
		}

		/// <inheritdoc/>
		public async Task<IEnumerable<UserDto>> GetPendingArtistsAsync()
		{
			// Query all users with role "Artist" who have not yet been approved.
			return await _db.Users
				.Where(u => u.Role == "Artist" && !u.IsApproved)
				.Select(u => new UserDto
				{
					Id = u.Id,
					Username = u.Username,
					Email = u.Email,
					Role = u.Role
				})
				.ToListAsync();
		}

		/// <inheritdoc/>
		public async Task<bool> ApproveArtistAsync(int artistId)
		{
			// Find the artist by ID
			var user = await _db.Users.FindAsync(artistId);
			if (user == null || user.IsApproved)
				return false;

			// Mark as approved
			user.IsApproved = true;
			await _db.SaveChangesAsync();
			return true;
		}

		/// <inheritdoc/>
		public async Task<bool> RejectArtistAsync(int artistId)
		{
			// Find the artist by ID
			var user = await _db.Users.FindAsync(artistId);
			if (user == null || user.IsApproved)
				return false;

			// Remove their account entirely
			_db.Users.Remove(user);
			await _db.SaveChangesAsync();
			return true;
		}

		/// <inheritdoc/>
		public async Task<IEnumerable<ArtworkDto>> GetPendingArtworksAsync()
		{
			// Query all artworks that haven't been approved yet
			return await _db.Artworks
				.Where(a => !a.IsApproved)
				.Select(a => ToDto(a))
				.ToListAsync();
		}

		/// <inheritdoc/>
		public async Task<bool> ApproveArtworkAsync(int artworkId)
		{
			// Find the artwork by ID
			var art = await _db.Artworks.FindAsync(artworkId);
			if (art == null || art.IsApproved)
				return false;

			// Mark as approved
			art.IsApproved = true;
			await _db.SaveChangesAsync();
			return true;
		}

		/// <inheritdoc/>
		public async Task<bool> RejectArtworkAsync(int artworkId)
		{
			// Find the artwork by ID
			var art = await _db.Artworks.FindAsync(artworkId);
			if (art == null || art.IsApproved)
				return false;

			// Remove the artwork record
			_db.Artworks.Remove(art);
			await _db.SaveChangesAsync();
			return true;
		}

		/// <summary>
		///     Maps an Artwork entity to its DTO representation.
		/// </summary>
		private static ArtworkDto ToDto(Artwork a) => new()
		{
			Id = a.Id,
			Title = a.Title,
			Description = a.Description,
			Price = a.Price,
			CreatedAt = a.CreatedAt,
			Category = a.Category,
			Tags = a.Tags,
			ImageUrl = a.ImageUrl,
			ArtistId = a.ArtistId,
			AuctionStartTime = a.AuctionStartTime,
			AuctionEndTime = a.AuctionEndTime
		};
	}
}
