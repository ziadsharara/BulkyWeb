using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Application.DTOs
{
	public class ArtworkFilterDto
	{
		public string? Category { get; set; }
		public string? Tag { get; set; }
		public string? SortBy { get; set; }
		public string? SortDirection { get; set; }
		public int PageNumber { get; set; } = 1;
		public int PageSize { get; set; } = 10;

		public decimal? MinPrice { get; set; }
		public decimal? MaxPrice { get; set; }
	}
}
