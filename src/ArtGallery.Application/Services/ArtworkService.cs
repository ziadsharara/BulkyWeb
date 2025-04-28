using ArtGallery.Application.DTOs;
using ArtGallery.Domain.Entities;
using ArtGallery.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtGallery.Application.Services
{
	public class ArtworkService : IArtworkService
	{
		private readonly AppDbContext _context;

		// Constructor: inject the database context
		public ArtworkService(AppDbContext context)
		{
			_context = context;
		}

		// --------------------------------------------------------------------------------
		// Creates a new artwork record, including auction times, and returns its DTO
		// --------------------------------------------------------------------------------
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
				ArtistId = userId,
				AuctionStartTime = dto.AuctionStartTime,
				AuctionEndTime = dto.AuctionEndTime
			};

			_context.Artworks.Add(artwork);
			await _context.SaveChangesAsync();
			return ToDto(artwork);
		}

		// --------------------------------------------------------------------------------
		// Retrieves artworks with filtering, sorting, and pagination
		// --------------------------------------------------------------------------------
		public async Task<PagedResult<ArtworkDto>> GetAllAsync(
				string? category,
				string? tag,
				string sortBy,
				string sortDirection,
				int pageNumber,
				int pageSize,
				string? searchTerm,
				string? artistId)
		{
			var query = _context.Artworks.AsQueryable();

			if (!string.IsNullOrWhiteSpace(category))
				query = query.Where(a => a.Category == category);

			if (!string.IsNullOrWhiteSpace(tag))
				query = query.Where(a => a.Tags.Contains(tag));

			if (!string.IsNullOrWhiteSpace(searchTerm))
				query = query.Where(a => a.Title.Contains(searchTerm) || a.Description.Contains(searchTerm));

			if (!string.IsNullOrWhiteSpace(artistId) && int.TryParse(artistId, out int artistIdInt))
				query = query.Where(a => a.ArtistId == artistIdInt);

			sortBy = sortBy?.ToLower() ?? "createdat";
			sortDirection = sortDirection?.ToLower() ?? "desc";

			query = (sortBy, sortDirection) switch
			{
				("price", "asc") => query.OrderBy(a => a.Price),
				("price", "desc") => query.OrderByDescending(a => a.Price),
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

		// --------------------------------------------------------------------------------
		// Filters and sorts artworks based on a filter DTO
		// --------------------------------------------------------------------------------
		public async Task<PagedResult<ArtworkDto>> FilterAndSortAsync(ArtworkFilterDto filter)
		{
			var query = _context.Artworks.AsQueryable();

			if (!string.IsNullOrEmpty(filter.Category))
				query = query.Where(a => a.Category == filter.Category);

			if (filter.MinPrice.HasValue)
				query = query.Where(a => a.Price >= filter.MinPrice);

			if (filter.MaxPrice.HasValue)
				query = query.Where(a => a.Price <= filter.MaxPrice);

			if (!string.IsNullOrEmpty(filter.Tags))
				query = query.Where(a => a.Tags.Contains(filter.Tags));

			query = filter.SortBy switch
			{
				"price" when filter.SortDirection == "asc" => query.OrderBy(a => a.Price),
				"price" when filter.SortDirection == "desc" => query.OrderByDescending(a => a.Price),
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

		// --------------------------------------------------------------------------------
		// Retrieves an artwork by its ID
		// --------------------------------------------------------------------------------
		public async Task<ArtworkDto?> GetByIdAsync(int id)
		{
			var artwork = await _context.Artworks.FindAsync(id);
			if (artwork == null) return null;
			return ToDto(artwork);
		}

		// --------------------------------------------------------------------------------
		// Updates an existing artwork; returns false if not found or unauthorized
		// --------------------------------------------------------------------------------
		public async Task<bool> UpdateAsync(int id, ArtworkDto dto)
		{
			var artwork = await _context.Artworks.FindAsync(id);
			if (artwork == null || artwork.ArtistId != dto.ArtistId)
				return false;

			artwork.Title = dto.Title;
			artwork.Description = dto.Description;
			artwork.Price = dto.Price;
			artwork.Category = dto.Category;
			artwork.Tags = dto.Tags;
			artwork.ImageUrl = dto.ImageUrl;

			await _context.SaveChangesAsync();
			return true;
		}

		// --------------------------------------------------------------------------------
		// Deletes an artwork by ID; returns false if not found
		// --------------------------------------------------------------------------------
		public async Task<bool> DeleteAsync(int id)
		{
			var artwork = await _context.Artworks.FindAsync(id);
			if (artwork == null) return false;

			_context.Artworks.Remove(artwork);
			await _context.SaveChangesAsync();
			return true;
		}

		// --------------------------------------------------------------------------------
		// Places a bid; must occur during auction window and meet minimum increment
		// --------------------------------------------------------------------------------
		public async Task<bool> PlaceBidAsync(int artworkId, int bidderId, decimal bidAmount)
		{
			var artwork = await _context.Artworks.FindAsync(artworkId);
			if (artwork == null) return false;

			var now = DateTime.UtcNow;

			if (now < artwork.AuctionStartTime || now > artwork.AuctionEndTime)
				return false;

			var lastBid = await _context.Bids
					.Where(b => b.ArtworkId == artworkId)
					.OrderByDescending(b => b.BidTime)
					.FirstOrDefaultAsync();

			var minimumBid = (lastBid?.Amount ?? artwork.Price) + 10;
			if (bidAmount < minimumBid) return false;

			_context.Bids.Add(new Bid
			{
				ArtworkId = artworkId,
				Amount = bidAmount,
				BidTime = now,
				BuyerId = bidderId
			});

			await _context.SaveChangesAsync();
			return true;
		}

		// --------------------------------------------------------------------------------
		// Retrieves the full bid history for a specific artwork
		// --------------------------------------------------------------------------------
		public async Task<IEnumerable<BidDto>> GetBidHistoryAsync(int artworkId)
		{
			return await _context.Bids
					.Where(b => b.ArtworkId == artworkId)
					.OrderBy(b => b.BidTime)
					.Select(b => new BidDto
					{
						ArtworkId = b.ArtworkId,
						BuyerId = b.BuyerId,
						Amount = b.Amount,
						BidTime = b.BidTime
					})
					.ToListAsync();
		}

		// --------------------------------------------------------------------------------
		// Retrieves the winning bid after auction ends; only the artist can access
		// --------------------------------------------------------------------------------
		public async Task<BidDto?> GetWinnerAsync(int artworkId, int artistId)
		{
			var artwork = await _context.Artworks.FindAsync(artworkId);
			if (artwork == null || artwork.ArtistId != artistId)
				return null;

			if (DateTime.UtcNow <= artwork.AuctionEndTime)
				return null;

			var lastBid = await _context.Bids
					.Where(b => b.ArtworkId == artworkId)
					.OrderByDescending(b => b.BidTime)
					.FirstOrDefaultAsync();

			if (lastBid == null) return null;

			return new BidDto
			{
				ArtworkId = lastBid.ArtworkId,
				BuyerId = lastBid.BuyerId,
				Amount = lastBid.Amount,
				BidTime = lastBid.BidTime
			};
		}

		// --------------------------------------------------------------------------------
		// Extends the auction end time for an artwork; only the artist can extend it
		// --------------------------------------------------------------------------------
		public async Task<bool> ExtendAuctionTimeAsync(int artworkId, DateTime newEndTime, int artistId)
		{
			var artwork = await _context.Artworks.FindAsync(artworkId);
			if (artwork == null || artwork.ArtistId != artistId || newEndTime <= DateTime.UtcNow)
				return false;

			artwork.AuctionEndTime = newEndTime;
			await _context.SaveChangesAsync();
			return true;
		}

		// --------------------------------------------------------------------------------
		// Converts the Artwork entity to its DTO representation
		// --------------------------------------------------------------------------------
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
			ArtistId = artwork.ArtistId,
			AuctionStartTime = artwork.AuctionStartTime,
			AuctionEndTime = artwork.AuctionEndTime
		};

		// --------------------------------------------------------------------------------
		// (Placeholder) Sets the bid timestamp (not implemented yet)
		// --------------------------------------------------------------------------------
		public void SetBidBidTime()
		{
			// Implement logic if needed
		}
	}
}
