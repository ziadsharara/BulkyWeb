using System;

namespace ArtGallery.Application.DTOs
{
	public class BidDto
	{
		// The ID of the artwork this bid belongs to
		public int ArtworkId { get; set; }

		// The ID of the user (buyer) who placed this bid
		public int BuyerId { get; set; }

		// The amount of the bid
		public decimal Amount { get; set; }

		// The UTC timestamp when the bid was placed
		public DateTime BidTime { get; set; }
	}

}
