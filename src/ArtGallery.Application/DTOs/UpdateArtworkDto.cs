using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Application.DTOs
{
	/// <summary>
	/// Data Transfer Object (DTO) used to update an existing artwork.
	/// Contains properties that can be modified by the user.
	/// </summary>
	public class UpdateArtworkDto
	{
		/// <summary>
		/// The title of the artwork.
		/// </summary>
		public string Title { get; set; } = string.Empty;

		/// <summary>
		/// A detailed description of the artwork.
		/// </summary>
		public string Description { get; set; } = string.Empty;

		/// <summary>
		/// The price of the artwork.
		/// </summary>
		public decimal Price { get; set; }

		/// <summary>
		/// The category or type of the artwork (e.g., painting, sculpture).
		/// </summary>
		public string Category { get; set; } = string.Empty;
	}
}

