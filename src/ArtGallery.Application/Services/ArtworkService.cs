using ArtGallery.Application.DTOs;
using ArtGallery.Application.Services;
using ArtGallery.Domain.Entities;
using ArtGallery.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Application.Services
{
	public class ArtworkService : IArtworkService
	{
		private readonly AppDbContext _context;

		public ArtworkService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<ArtworkDto> CreateAsync(ArtworkDto artworkDto)
		{
			if (!await _context.Users.AnyAsync(u => u.Id == artworkDto.ArtistId))
				throw new KeyNotFoundException($"Artist with ID {artworkDto.ArtistId} not found.");

			var artwork = new Artwork
			{
				Title = artworkDto.Title,
				Description = artworkDto.Description,
				Price = artworkDto.Price,
				CreatedAt = DateTime.UtcNow,
				Category = artworkDto.Category,
				ImageUrl = artworkDto.ImageUrl,
				Tags = artworkDto.Tags,
				ArtistId = artworkDto.ArtistId
			};

			_context.Artworks.Add(artwork);
			await _context.SaveChangesAsync();

			return ToDto(artwork);
		}

		public async Task<IEnumerable<ArtworkDto>> GetAllAsync()
		{
			var artworks = await _context.Artworks.ToListAsync();
			return artworks.Select(ToDto);
		}

		public async Task<ArtworkDto?> GetByIdAsync(int id)
		{
			var artwork = await _context.Artworks.FindAsync(id);
			return artwork is null ? null : ToDto(artwork);
		}

		public async Task<bool> UpdateAsync(int id, ArtworkDto dto)
		{
			var artwork = await _context.Artworks.FindAsync(id);
			if (artwork == null) return false;

			if (!await _context.Users.AnyAsync(u => u.Id == dto.ArtistId))
				return false;

			artwork.Title = dto.Title;
			artwork.Description = dto.Description;
			artwork.Price = dto.Price;
			artwork.CreatedAt = dto.CreatedAt; // Optional: could ignore if server manages CreatedAt
			artwork.Category = dto.Category;
			artwork.ImageUrl = dto.ImageUrl;
			artwork.Tags = dto.Tags;
			artwork.ArtistId = dto.ArtistId;

			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var artwork = await _context.Artworks.FindAsync(id);
			if (artwork == null) return false;

			_context.Artworks.Remove(artwork);
			await _context.SaveChangesAsync();
			return true;
		}

		private static ArtworkDto ToDto(Artwork artwork) => new()
		{
			Id = artwork.Id,
			Title = artwork.Title,
			Description = artwork.Description,
			Price = artwork.Price,
			CreatedAt = artwork.CreatedAt,
			Category = artwork.Category,
			ImageUrl = artwork.ImageUrl,
			Tags = artwork.Tags,
			ArtistId = artwork.ArtistId
		};
	}
}
