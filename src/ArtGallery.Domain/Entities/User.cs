using System;
using System.Collections.Generic;

namespace ArtGallery.Domain.Entities
{
	public class User
	{
		// --------------------------------------------------------------------------------
		// Unique identifier for each user
		// --------------------------------------------------------------------------------
		public int Id { get; set; }

		// --------------------------------------------------------------------------------
		// The username chosen by the user, which must be unique
		// --------------------------------------------------------------------------------
		public string Username { get; set; } = default!;

		// --------------------------------------------------------------------------------
		// The hashed password for secure authentication
		// --------------------------------------------------------------------------------
		public string PasswordHash { get; set; } = default!;

		// --------------------------------------------------------------------------------
		// The email address associated with the user, should be unique
		// --------------------------------------------------------------------------------
		public string Email { get; set; } = default!;

		// --------------------------------------------------------------------------------
		// The role of the user, which can be "Admin", "Artist", or "Buyer"
		// --------------------------------------------------------------------------------
		public string Role { get; set; } = default!;

		// --------------------------------------------------------------------------------
		// Collection of artworks created by the user (only if the user is an artist)
		// --------------------------------------------------------------------------------
		public ICollection<Artwork> Artworks { get; set; } = new List<Artwork>();

		// --------------------------------------------------------------------------------
		// Collection of bids placed by the user (only if the user is a buyer)
		// --------------------------------------------------------------------------------
		public ICollection<Bid> Bids { get; set; } = new List<Bid>();

		// --------------------------------------------------------------------------------
		// Determines if the user is an artist (based on their role)
		// --------------------------------------------------------------------------------
		public bool IsArtist => Role == "Artist"; // Checks if the user's role is "Artist"

		// --------------------------------------------------------------------------------
		// Determines if the user is an admin (based on their role)
		// --------------------------------------------------------------------------------
		public bool IsAdmin => Role == "Admin"; // Checks if the user's role is "Admin"

		// --------------------------------------------------------------------------------
		// Determines if the user is a buyer (based on their role)
		// --------------------------------------------------------------------------------
		public bool IsBuyer => Role == "Buyer"; // Checks if the user's role is "Buyer"

		// --------------------------------------------------------------------------------
		// Flag to determine if the artist is approved by an admin
		// --------------------------------------------------------------------------------
		public bool IsApproved { get; set; } = false;
	}
}
