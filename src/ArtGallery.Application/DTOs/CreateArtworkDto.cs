using System;
using System.Collections.Generic;

namespace ArtGallery.Application.DTOs
{
	public class CreateArtworkDto
	{
		public string Title { get; set; } // Title of the artwork
		public string Description { get; set; } // Description of the artwork
		public decimal Price { get; set; } // Initial price of the artwork
		public string Category { get; set; } // Category of the artwork (e.g., Portrait, Landscape)

		// -------------------------------------------------------------------
		// Store multiple tags as a list of keywords (e.g. ["modern","oil","vibrant"])
		// -------------------------------------------------------------------
		public List<string>? Tags { get; set; } // Tags to describe the artwork

		public string? ImageUrl { get; set; } // URL to the image of the artwork

		// -------------------------------------------------------------------
		// Auction start time – when bidding opens
		// -------------------------------------------------------------------
		public DateTime AuctionStartTime { get; set; } // Start time of the auction

		// -------------------------------------------------------------------
		// Auction end time – when bidding closes
		// -------------------------------------------------------------------
		public DateTime AuctionEndTime { get; set; } // End time of the auction
	}
}
