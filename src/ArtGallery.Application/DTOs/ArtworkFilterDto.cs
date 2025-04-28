namespace ArtGallery.Application.DTOs
{
	// --------------------------------------------------------------
	// DTO used for filtering artwork in the gallery
	// --------------------------------------------------------------
	public class ArtworkFilterDto
	{
		// --------------------------------------------------------------
		// The category by which the artwork is filtered
		// --------------------------------------------------------------
		public string? Category { get; set; }

		// --------------------------------------------------------------
		// The tag by which the artwork is filtered
		// --------------------------------------------------------------
		public string? Tags { get; set; }

		// --------------------------------------------------------------
		// The sorting criteria (e.g., "Price", "Date", or "Title")
		// --------------------------------------------------------------
		public string? SortBy { get; set; }

		// --------------------------------------------------------------
		// The sorting direction (e.g., "asc" or "desc")
		// --------------------------------------------------------------
		public string? SortDirection { get; set; }

		// --------------------------------------------------------------
		// The current page number for paginating the artwork results
		// Pagination is done based on this number and the page size
		// --------------------------------------------------------------
		public int PageNumber { get; set; } = 1;

		// --------------------------------------------------------------
		// The page size (the number of items to display per page)
		// --------------------------------------------------------------
		public int PageSize { get; set; } = 10;

		// --------------------------------------------------------------
		// The minimum price by which the artwork is filtered
		// --------------------------------------------------------------
		public decimal? MinPrice { get; set; }

		// --------------------------------------------------------------
		// The maximum price by which the artwork is filtered
		// --------------------------------------------------------------
		public decimal? MaxPrice { get; set; }
	}
}
