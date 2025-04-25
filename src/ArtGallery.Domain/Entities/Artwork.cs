using System;
using System.Collections.Generic;

namespace ArtGallery.Domain.Entities
{
	public class Artwork
	{
		public int Id { get; set; }
		public string Title { get; set; } = default!;
		public string Description { get; set; } = default!;
		public decimal Price { get; set; }
		public DateTime CreatedAt { get; set; }
		public string Category { get; set; } = default!;
		public string ImageUrl { get; set; } = default!;
		public List<string> Tags { get; set; } = new();

		public int ArtistId { get; set; }
		public User Artist { get; set; } = default!;

		public ICollection<Bid> Bids { get; set; } = new List<Bid>();
		public ICollection<ArtworkLike> Likes { get; set; } = new List<ArtworkLike>();
	}

}
