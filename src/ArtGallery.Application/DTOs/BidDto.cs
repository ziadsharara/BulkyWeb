using System;

namespace ArtGallery.Application.DTOs
{
	public class BidDto
	{
		public int ArtworkId { get; set; }
		public int BuyerId { get; set; }
		public decimal Amount { get; set; }
		public DateTime Timestamp { get;  set; }

	}
}
