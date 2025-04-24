using ArtGallery.Application.DTOs;
using ArtGallery.Domain.Entities;
using ArtGallery.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ArtGallery.Application.Services
{
	public class ArtworkService : IArtworkService
	{
		private readonly AppDbContext _context;
		public ArtworkService(AppDbContext context) => _context = context;

		public async Task<ArtworkDto> CreateAsync(CreateArtworkDto dto)
		{
			if (!await _context.Users.AnyAsync(u => u.Id == dto.ArtistId))
				throw new KeyNotFoundException($"Artist with ID {dto.ArtistId} not found.");

			var artwork = new Artwork
			{
				Title = dto.Title,
				Description = dto.Description,
				Price = dto.Price,
				CreatedAt = DateTime.UtcNow,
				Category = dto.Category,
				ImageUrl = dto.ImageUrl,
				Tags = dto.Tags,
				ArtistId = dto.ArtistId
			};

			_context.Artworks.Add(artwork);
			await _context.SaveChangesAsync();
			return ToDto(artwork);
		}

		public async Task<PagedResult<ArtworkDto>> GetAllAsync(
				string? category,
				string? tag,
				string sortBy,
				string sortDirection,
				int pageNumber,
				int pageSize
		)
		{
			var query = _context.Artworks.AsQueryable();

			if (!string.IsNullOrWhiteSpace(category))
				query = query.Where(a => a.Category == category);

			if (!string.IsNullOrWhiteSpace(tag))
				query = query.Where(a => a.Tags.Contains(tag));

			query = (sortBy.ToLower(), sortDirection.ToLower()) switch
			{
				("price", "asc") => query.OrderBy(a => a.Price),
				("price", "desc") => query.OrderByDescending(a => a.Price),
				(_, "asc") => query.OrderBy(a => a.CreatedAt),
				_ => query.OrderByDescending(a => a.CreatedAt),
			};

			var totalCount = await query.CountAsync();
			var items = await query
					.Skip((pageNumber - 1) * pageSize)
					.Take(pageSize)
					.Select(a => ToDto(a))
					.ToListAsync();

			return new PagedResult<ArtworkDto>
			{
				PageNumber = pageNumber,
				PageSize = pageSize,
				TotalCount = totalCount,
				Items = items
			};
		}

		public async Task<ArtworkDto?> GetByIdAsync(int id)
		{
			var a = await _context.Artworks.FindAsync(id);
			return a is null ? null : ToDto(a);
		}

		public async Task<bool> UpdateAsync(int id, ArtworkDto dto)
		{
			var a = await _context.Artworks.FindAsync(id);
			if (a == null) return false;
			if (!await _context.Users.AnyAsync(u => u.Id == dto.ArtistId))
				return false;

			a.Title = dto.Title;
			a.Description = dto.Description;
			a.Price = dto.Price;
			a.CreatedAt = dto.CreatedAt;
			a.Category = dto.Category;
			a.ImageUrl = dto.ImageUrl;
			a.Tags = dto.Tags;
			a.ArtistId = dto.ArtistId;

			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var a = await _context.Artworks.FindAsync(id);
			if (a == null) return false;
			_context.Artworks.Remove(a);
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
