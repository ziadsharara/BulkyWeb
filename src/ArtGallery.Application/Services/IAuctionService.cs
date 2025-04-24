using ArtGallery.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtGallery.Application.Services
{
	public interface IAuctionService
	{
		Task<BidDto> PlaceBidAsync(BidDto bidDto);
		Task<IEnumerable<BidDto>> GetBidsForArtworkAsync(int artworkId);
	}
}
