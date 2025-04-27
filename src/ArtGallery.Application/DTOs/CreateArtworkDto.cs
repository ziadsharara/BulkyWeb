using System.Collections.Generic;

namespace ArtGallery.Application.DTOs
{
	public class CreateArtworkDto
	{
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public decimal Price { get; set; }
		public string Category { get; set; } = string.Empty;
		public List<string> Tags { get; set; } = new();
		public string? ImageUrl { get; set; }
		public int ArtistId { get; set; }
	}
}
