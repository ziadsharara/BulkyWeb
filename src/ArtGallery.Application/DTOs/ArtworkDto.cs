using System;
using System.Collections.Generic;

namespace ArtGallery.Application.DTOs
{
	public class ArtworkDto
	{
		public int Id { get; set; } // ID of the artwork
		public string Title { get; set; } // Title of the artwork
		public string Description { get; set; } // Description of the artwork
		public decimal Price { get; set; } // Price of the artwork
		public DateTime CreatedAt { get; set; } // Date when the artwork was created
		public string Category { get; set; } // Category of the artwork (e.g., Portrait, Landscape)

		// Expose tags to the client as a list of keywords
		public List<string>? Tags { get; set; } // Tags for artwork

		public string? ImageUrl { get; set; } // Image URL of the artwork
		public int ArtistId { get; set; } // ID of the artist

		// Auction start time – included for client display
		public DateTime AuctionStartTime { get; set; } // Auction start time

		// Auction end time – included for client display
		public DateTime AuctionEndTime { get; set; } // Auction end time
	}
}
