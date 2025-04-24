using System;

namespace ArtGallery.Domain.Entities
{
	public class Bid
	{
		public int Id { get; set; }
		public decimal Amount { get; set; }
		public DateTime Timestamp { get; set; }

		public int ArtworkId { get; set; }
		public Artwork Artwork { get; set; } = default!;

		public int BuyerId { get; set; }
		public User Buyer { get; set; } = default!;
	}
}
