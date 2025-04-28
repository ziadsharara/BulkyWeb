using ArtGallery.Application.DTOs;
using ArtGallery.Infrastructure.Data;
using ArtGallery.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtGallery.Application.Services
{
	// AuctionService: This service handles the business logic for placing and retrieving bids.
	public class AuctionService : IAuctionService
	{
		private readonly AppDbContext _db;

		// Constructor to inject the AppDbContext for database access
		public AuctionService(AppDbContext db)
		{
			_db = db;
		}

		// --------------------------------------------------------------------------------
		// PlaceBidAsync: Places a new bid on an artwork. 
		// Validates if the artwork and buyer exist, and ensures the bid is higher than the 
		// current highest bid, with a minimum bid increment of 10.
		// --------------------------------------------------------------------------------
		public async Task<BidDto> PlaceBidAsync(BidDto bidDto)
		{
			// Validate if the artwork exists
			if (!await _db.Artworks.AnyAsync(a => a.Id == bidDto.ArtworkId))
				throw new KeyNotFoundException($"Artwork with id {bidDto.ArtworkId} not found.");

			// Validate if the buyer exists
			if (!await _db.Users.AnyAsync(u => u.Id == bidDto.BuyerId))
				throw new KeyNotFoundException($"Buyer with id {bidDto.BuyerId} not found.");

			// Get the highest bid for the artwork
			var highestBid = await _db.Bids
					.Where(b => b.ArtworkId == bidDto.ArtworkId)
					.OrderByDescending(b => b.Amount)
					.FirstOrDefaultAsync();

			// Set the minimum bid amount (either 10 higher than the current bid or the initial bid)
			var minBidAmount = highestBid != null ? highestBid.Amount + 10 : bidDto.Amount;

			// Ensure the new bid is higher than the current highest bid
			if (bidDto.Amount < minBidAmount)
				throw new InvalidOperationException($"Bid must be at least {minBidAmount}");

			// Create the new bid
			var bid = new Bid
			{
				ArtworkId = bidDto.ArtworkId,
				BuyerId = bidDto.BuyerId,
				Amount = bidDto.Amount,
				BidTime = DateTime.UtcNow
			};

			// Add the bid to the database and save changes
			_db.Bids.Add(bid);
			await _db.SaveChangesAsync();

			// Return the newly created bid as a DTO
			return new BidDto
			{
				ArtworkId = bid.ArtworkId,
				BuyerId = bid.BuyerId,
				Amount = bid.Amount,
				BidTime = bid.BidTime
			};
		}

		// --------------------------------------------------------------------------------
		// GetBidsForArtworkAsync: Retrieves all bids for a specific artwork.
		// --------------------------------------------------------------------------------
		public async Task<IEnumerable<BidDto>> GetBidsForArtworkAsync(int artworkId)
		{
			// Validate if the artwork exists
			if (!await _db.Artworks.AnyAsync(a => a.Id == artworkId))
				throw new KeyNotFoundException($"Artwork with id {artworkId} not found.");

			// Retrieve all bids for the artwork and map them to DTOs
			return await _db.Bids
					.Where(b => b.ArtworkId == artworkId)
					.Select(b => new BidDto
					{
						ArtworkId = b.ArtworkId,
						BuyerId = b.BuyerId,
						Amount = b.Amount,
						BidTime = b.BidTime
					})
					.ToListAsync();
		}
	}
}
