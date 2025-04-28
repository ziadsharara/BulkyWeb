namespace ArtGallery.Application.DTOs
{
	// -------------------------------------------------------------------
	// A generic class that represents a paginated result.
	// It contains metadata about the pagination as well as the list of items.
	// -------------------------------------------------------------------
	public class PagedResult<T>
	{
		// -------------------------------------------------------------------
		// The current page number in the paginated result.
		// Page number should start from 1.
		// -------------------------------------------------------------------
		public int PageNumber { get; set; }

		// -------------------------------------------------------------------
		// The number of items per page.
		// It controls how many items should be returned in a single page.
		// -------------------------------------------------------------------
		public int PageSize { get; set; }

		// -------------------------------------------------------------------
		// The total number of items available across all pages.
		// This is used to calculate the total number of pages.
		// -------------------------------------------------------------------
		public int TotalCount { get; set; }

		// -------------------------------------------------------------------
		// The list of items for the current page.
		// This is a collection of the results returned for this page.
		// -------------------------------------------------------------------
		public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
	}
}
