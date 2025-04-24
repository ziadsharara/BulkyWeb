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
	public class AuctionService : IAuctionService
	{
		private readonly AppDbContext _db;

		public AuctionService(AppDbContext db)
		{
			_db = db;
		}

		public async Task<BidDto> PlaceBidAsync(BidDto bidDto)
		{
			// Verify artwork exists
			if (!await _db.Artworks.AnyAsync(a => a.Id == bidDto.ArtworkId))
				throw new KeyNotFoundException($"Artwork with id {bidDto.ArtworkId} not found.");

			// Verify buyer exists
			if (!await _db.Users.AnyAsync(u => u.Id == bidDto.BuyerId))
				throw new KeyNotFoundException($"Buyer with id {bidDto.BuyerId} not found.");

			var highestBid = await _db.Bids
					.Where(b => b.ArtworkId == bidDto.ArtworkId)
					.OrderByDescending(b => b.Amount)
					.FirstOrDefaultAsync();

			var minBidAmount = highestBid != null ? highestBid.Amount + 10 : bidDto.Amount;

			if (bidDto.Amount < minBidAmount)
				throw new InvalidOperationException($"Bid must be at least {minBidAmount}");

			var bid = new Bid
			{
				ArtworkId = bidDto.ArtworkId,
				BuyerId = bidDto.BuyerId,
				Amount = bidDto.Amount,
				Timestamp = DateTime.UtcNow
			};

			_db.Bids.Add(bid);
			await _db.SaveChangesAsync();

			return new BidDto
			{
				ArtworkId = bid.ArtworkId,
				BuyerId = bid.BuyerId,
				Amount = bid.Amount,
				Timestamp = bid.Timestamp
			};
		}

		public async Task<IEnumerable<BidDto>> GetBidsForArtworkAsync(int artworkId)
		{
			// Verify artwork exists
			if (!await _db.Artworks.AnyAsync(a => a.Id == artworkId))
				throw new KeyNotFoundException($"Artwork with id {artworkId} not found.");

			return await _db.Bids
					.Where(b => b.ArtworkId == artworkId)
					.Select(b => new BidDto
					{
						ArtworkId = b.ArtworkId,
						BuyerId = b.BuyerId,
						Amount = b.Amount,
						Timestamp = b.Timestamp
					})
					.ToListAsync();
		}
	}
}
