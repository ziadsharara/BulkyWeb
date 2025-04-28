using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Application.DTOs
{
	/// <summary>
	/// Parameters used for querying artworks with filtering, sorting, and pagination.
	/// </summary>
	public class ArtworkQueryParameters
	{
		private const int MaxPageSize = 50;

		private int _pageSize = 10;

		/// <summary>
		/// Page number for pagination. Default is 1.
		/// </summary>
		public int PageNumber { get; set; } = 1;

		/// <summary>
		/// Number of items per page. Default is 10. Max is 50.
		/// </summary>
		public int PageSize
		{
			get => _pageSize;
			set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
		}

		/// <summary>
		/// Optional filter by artwork title.
		/// </summary>
		public string? Title { get; set; }

		/// <summary>
		/// Optional filter by artwork description.
		/// </summary>
		public string? Description { get; set; }

		/// <summary>
		/// Optional field name to sort by.
		/// Example: "title" or "createdAt".
		/// </summary>
		public string? SortBy { get; set; }

		/// <summary>
		/// Sort order: "asc" for ascending or "desc" for descending.
		/// Default is ascending.
		/// </summary>
		public string SortOrder { get; set; } = "asc";
	}
}
