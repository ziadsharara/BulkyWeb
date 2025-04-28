using System;

namespace ArtGallery.Domain.Entities
{
	/// <summary>
	/// Represents a bid placed by a buyer on an artwork.
	/// </summary>
	public class Bid
	{
		/// <summary>
		/// Unique identifier of the bid.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Foreign key to the artwork being bid on.
		/// </summary>
		public int ArtworkId { get; set; }

		/// <summary>
		/// Foreign key to the buyer who placed this bid.
		/// </summary>
		public int BuyerId { get; set; }

		/// <summary>
		/// The monetary amount of the bid.
		/// </summary>
		public decimal Amount { get; set; }

		/// <summary>
		/// UTC timestamp when the bid was placed.
		/// </summary>
		public DateTime BidTime { get; set; }

		/// <summary>
		/// Navigation property to the associated artwork.
		/// </summary>
		public virtual Artwork Artwork { get; set; } = default!;

		/// <summary>
		/// Navigation property to the buyer (user) who placed the bid.
		/// </summary>
		public virtual User Buyer { get; set; } = default!;

		/// <summary>
		/// Validates that this bid's amount is strictly greater than the provided last bid amount.
		/// </summary>
		/// <param name="lastBidAmount">The amount of the previous highest bid.</param>
		/// <returns>True if this bid is valid (higher); otherwise false.</returns>
		public bool IsValidBid(decimal lastBidAmount)
		{
			return Amount > lastBidAmount;
		}

		/// <summary>
		/// Sets the bid timestamp to the current UTC time.
		/// </summary>
		public void SetBidTime()
		{
			BidTime = DateTime.UtcNow;
		}
	}
}
