using System;
using System.Collections.Generic;

namespace ArtGallery.Application.DTOs
{
	public class ArtworkDto
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
	}
}
