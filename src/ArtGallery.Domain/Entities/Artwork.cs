using System;
using System.Collections.Generic;

namespace ArtGallery.Domain.Entities
{
	public class Artwork
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public DateTime CreatedAt { get; set; }
		public string Category { get; set; }

		// Changed Tags to List<string> for consistent DTO mapping
		public List<string>? Tags { get; set; }

		public string? ImageUrl { get; set; }
		public int ArtistId { get; set; }

		// Auction start and end times for bidding window
		public DateTime AuctionStartTime { get; set; }
		public DateTime AuctionEndTime { get; set; }

		// Navigation properties
		public ICollection<Bid> Bids { get; set; }

		// NEW navigation to the User (artist) who created this artwork
		public User Artist { get; set; }

		// Method to ensure AuctionStartTime is before AuctionEndTime
		public void SetAuctionTimes(DateTime start, DateTime end)
		{
			if (start >= end)
				throw new InvalidOperationException("Auction start time must be before end time.");

			AuctionStartTime = start;
			AuctionEndTime = end;
		}

		/// <summary>
		///     Only artworks that have been approved by an admin show up to buyers.
		/// </summary>
		public bool IsApproved { get; set; } = false;
	}
}
