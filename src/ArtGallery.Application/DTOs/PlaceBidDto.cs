namespace ArtGallery.Application.DTOs
{
	// -------------------------------------------------------------------
	// DTO class used to represent the data for placing a bid.
	// It contains the bid amount provided by the buyer.
	// -------------------------------------------------------------------
	public class PlaceBidDto
	{
		// -------------------------------------------------------------------
		// The amount the buyer wants to bid for the artwork.
		// It must be a positive number, and typically greater than or equal
		// to the minimum bid amount (if any).
		// -------------------------------------------------------------------
		public decimal BidAmount { get; set; }
	}
}
