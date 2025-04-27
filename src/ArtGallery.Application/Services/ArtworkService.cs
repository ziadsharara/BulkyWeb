using ArtGallery.Application.DTOs;
using ArtGallery.Domain.Entities;
using ArtGallery.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ArtGallery.Application.Services
{
	public class ArtworkService : IArtworkService
	{
		private readonly AppDbContext _context;

		public ArtworkService(AppDbContext context) => _context = context;

		public async Task<ArtworkDto> CreateAsync(CreateArtworkDto dto, int userId)
		{
			if (!await _context.Users.AnyAsync(u => u.Id == userId))
				throw new KeyNotFoundException($"User with ID {userId} not found.");

			var artwork = new Artwork
			{
				Title = dto.Title,
				Description = dto.Description,
				Price = dto.Price,
				CreatedAt = DateTime.UtcNow,
				Category = dto.Category,
				ImageUrl = dto.ImageUrl,
				Tags = dto.Tags,
				ArtistId = userId
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

			sortBy = sortBy?.ToLower() ?? "createdat";
			sortDirection = sortDirection?.ToLower() ?? "desc";

			query = (sortBy, sortDirection) switch
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

		public async Task<PagedResult<ArtworkDto>> FilterAndSortAsync(ArtworkFilterDto filter)
		{
			var query = _context.Artworks.AsQueryable();

			if (!string.IsNullOrWhiteSpace(filter.Category))
				query = query.Where(a => a.Category == filter.Category);

			if (!string.IsNullOrWhiteSpace(filter.Tag))
				query = query.Where(a => a.Tags.Contains(filter.Tag));

			if (filter.MinPrice.HasValue)
				query = query.Where(a => a.Price >= filter.MinPrice.Value);

			if (filter.MaxPrice.HasValue)
				query = query.Where(a => a.Price <= filter.MaxPrice.Value);

			var sortBy = filter.SortBy?.ToLower() ?? "createdat";
			var sortDirection = filter.SortDirection?.ToLower() ?? "desc";

			query = (sortBy, sortDirection) switch
			{
				("price", "asc") => query.OrderBy(a => a.Price),
				("price", "desc") => query.OrderByDescending(a => a.Price),
				(_, "asc") => query.OrderBy(a => a.CreatedAt),
				_ => query.OrderByDescending(a => a.CreatedAt),
			};

			var totalCount = await query.CountAsync();
			var items = await query
				.Skip((filter.PageNumber - 1) * filter.PageSize)
				.Take(filter.PageSize)
				.Select(a => ToDto(a))
				.ToListAsync();

			return new PagedResult<ArtworkDto>
			{
				PageNumber = filter.PageNumber,
				PageSize = filter.PageSize,
				TotalCount = totalCount,
				Items = items
			};
		}

		public async Task<ArtworkDto?> GetByIdAsync(int id)
		{
			var artwork = await _context.Artworks.SingleOrDefaultAsync(a => a.Id == id);
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
			artwork.CreatedAt = dto.CreatedAt;
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
