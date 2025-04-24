using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Domain.Entities
{
	public class User
	{
		public int Id { get; set; }
		public string Username { get; set; } = default!;
		public string PasswordHash { get; set; } = default!;
		public string Email { get; set; } = default!;
		public string Role { get; set; } = default!; // "Admin","Artist","Buyer"
		public ICollection<Artwork> Artworks { get; set; }
		public ICollection<Bid> Bids { get; set; }

	}
}
