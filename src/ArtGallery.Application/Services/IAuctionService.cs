using ArtGallery.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtGallery.Application.Services
{
	public interface IAuctionService
	{
		// Places a bid for an artwork. The bid must meet certain criteria (e.g., higher than the last bid or initial price)
		Task<BidDto> PlaceBidAsync(BidDto bidDto);

		// Retrieves all the bids placed for a specific artwork
		// Returns a collection of Bid DTOs associated with the given artwork ID
		Task<IEnumerable<BidDto>> GetBidsForArtworkAsync(int artworkId);
	}
}
