using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArtGallery.Application.DTOs;

namespace ArtGallery.Application.Services
{
	/// <summary>
	/// Provides operations related to artworks, including CRUD, bidding, and auction management.
	/// </summary>
	public interface IArtworkService
	{
		Task<ArtworkDto> CreateAsync(CreateArtworkDto dto, int userId);

		Task<PagedResult<ArtworkDto>> GetAllAsync(
				string? category,
				string? tag,
				string sortBy,
				string sortDirection,
				int pageNumber,
				int pageSize,
				string? artistName = null,
				string? tags = null);

		Task<PagedResult<ArtworkDto>> FilterAndSortAsync(ArtworkFilterDto filter);

		Task<ArtworkDto?> GetByIdAsync(int id);

		Task<bool> UpdateAsync(int id, ArtworkDto dto);

		Task<bool> DeleteAsync(int id);

		Task<bool> PlaceBidAsync(int artworkId, int bidderId, decimal bidAmount);

		Task<IEnumerable<BidDto>> GetBidHistoryAsync(int artworkId);

		Task<bool> ExtendAuctionTimeAsync(int artworkId, DateTime newEndTime, int artistId);

		Task<BidDto?> GetWinnerAsync(int artworkId, int artistId);
	}
}
